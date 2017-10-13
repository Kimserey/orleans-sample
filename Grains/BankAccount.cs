using Interfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    public class BankAccount : Grain, IBankAccount
    {
        double _balance;

        public Task Deposit(double a)
        {
            _balance += a;
            return Task.CompletedTask;
        }

        public Task Withdraw(double a)
        {
            if (a > _balance)
                throw new Exception("Balance cannot be inferior to zero.");

            _balance -= a;
            return Task.CompletedTask;
        }

        public Task<double> GetBalance()
        {
            return Task.FromResult(_balance);
        }
    }
}
