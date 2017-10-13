using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IBankAccount: IGrainWithStringKey
    {
        Task Deposit(double a);
        Task Withdraw(double a);
        Task<double> GetBalance();
    }
}
