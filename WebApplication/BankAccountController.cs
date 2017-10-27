using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication
{
    [Route("Bank")]
    public class BankAccountController: Controller
    {
        private IGrainFactory _factory;

        public BankAccountController(IGrainFactory factory)
        {
            _factory = factory;
        }

        [HttpPost("{accountName}/Deposit")]
        public async Task<IActionResult> Deposit(string accountName, [FromBody]double amount)
        {
            var bank = _factory.GetGrain<IBankAccount>(accountName);
            await bank.Deposit(amount);
            return Ok();
        }

        [HttpPost("{accountName}/Withdraw")]
        public async Task<IActionResult> Withdraw(string accountName, [FromBody]double amount)
        {
            var bank = _factory.GetGrain<IBankAccount>(accountName);
            await bank.Withdraw(amount);
            return Ok();
        }


        [HttpGet("{accountName}")]
        [ProducesResponseType(typeof(double), 200)]
        public async Task<IActionResult> Get(string accountName)
        {
            var bank = _factory.GetGrain<IBankAccount>(accountName);
            var balance = await bank.GetBalance();
            return Ok(balance);
        }
    }
}
