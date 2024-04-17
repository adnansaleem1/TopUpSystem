using FinancialService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialService.Data
{
    public static class Seed
    {
        public static void SeedData(FinancialContext context)
        {
            SeedAccounts(context);
            SeedTransactions(context);
        }

        private static void SeedAccounts(FinancialContext context)
        {
            if (!context.Accounts.Any())
            {
                var accounts = new[]
                {
                new Account {Name = "User", Balance = 5000m, AccountType = "Checking", Currency = "AED", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Account {Name = "Beneficary", Balance = 1000m, AccountType = "Checking", Currency = "AED", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Account {Name = "TopUpVendor", Balance = 2000m, AccountType = "Checking", Currency = "AED", IsActive = true, CreatedDate = DateTime.UtcNow },
                new Account {Name = "Company", Balance = 2000m, AccountType = "Checking", Currency = "AED", IsActive = true, CreatedDate = DateTime.UtcNow }

            };
                context.Accounts.AddRange(accounts);
                context.SaveChanges();
            }
        }

        private static void SeedTransactions(FinancialContext context)
        {
            if (!context.Transactions.Any())
            {
                var transactions = new[]
                {
                new Transaction {  AccountId = 1,  Amount = 1000m, TransactionDate = DateTime.UtcNow, IsCredit = true, TransactionType = "Initial Credit", Status = "Completed" },
                new Transaction {  AccountId = 1, Amount = 500m, TransactionDate = DateTime.UtcNow, IsCredit = false, TransactionType = "Withdrawal", Status = "Completed" }
            };
                context.Transactions.AddRange(transactions);
                context.SaveChanges();
            }
        }
    }
}
