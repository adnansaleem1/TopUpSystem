using CallCredit.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CallCredit.Data
{
    public static class Seed
    {
        public static void SeedData(CallCreditContext context)
        {
           
            context.Database.EnsureCreated();
           
            if (!context.Users.Any())
            {
                SeedUsers(context);
            }

            if (!context.Beneficiaries.Any())
            {
                SeedBeneficiaries(context);
            }

            if (!context.TopUpTransactions.Any())
            {
                SeedTransactions(context);
            }

            if (!context.TopUpOptions.Any())
            {
                SeedTopUpOptions(context);
            }
        }

        private static void SeedUsers(CallCreditContext context)
        {
            var accountHolders = new[]
            {
                new User { IsVerified = true, AccountBalance = 1000m }               
            };
            context.Users.AddRange(accountHolders);
            context.SaveChanges();
        }

        private static void SeedBeneficiaries(CallCreditContext context)
        {
            var beneficiaries = new[]
            {
                new Beneficiary {  UserId = 1, Nickname = "John Doe",IsActive=true,PhoneNumber="+971501234567" },
                new Beneficiary { UserId = 1, Nickname = "Jane Smith",IsActive=true,PhoneNumber="+971552345678" }
            };
            context.Beneficiaries.AddRange(beneficiaries);
            context.SaveChanges();
        }

        private static void SeedTransactions(CallCreditContext context)
        {
            var transactions = new[]
            {
                new TopUpTransaction {  UserId=1, BeneficiaryId = 1, Amount = 100m, TransactionDate = DateTime.Now },
                new TopUpTransaction {  UserId=1, BeneficiaryId = 2, Amount = 150m, TransactionDate = DateTime.Now.AddDays(-1) }
            };
            context.TopUpTransactions.AddRange(transactions);
            context.SaveChanges();
        }

        public static void SeedTopUpOptions(CallCreditContext context)
        {
            var topUpOptions = new[]
            {
                new TopUpOption { Amount = 5m, Description =  "AED 5" },
                new TopUpOption { Amount = 10m, Description = "AED 10" },
                new TopUpOption { Amount = 20m, Description = "AED 20" },
                new TopUpOption { Amount = 30m, Description = "AED 30" },
                new TopUpOption { Amount = 50m, Description = "AED 50" },
                new TopUpOption { Amount = 75m, Description = "AED 75" },
                new TopUpOption { Amount = 100m, Description= "AED 100" }
            };
            context.TopUpOptions.AddRange(topUpOptions);
            context.SaveChanges();

        }
    }
}
