using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialService.Data.Entities
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
