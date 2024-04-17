using CallCredit.API.Interfaces;
using FinancialService.Data;
using FinancialService.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialService.API.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly FinancialContext _context;
        private readonly ILogger<AccountsService> _logger;

        public AccountsService(FinancialContext context, ILogger<AccountsService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> Credit(TransactionRequest request)
        {
            Account account = await GetAccount(request);

            account.Balance += request.Amount;
            var transaction = new Transaction
            {
                AccountId = account.AccountId,
                Amount = request.Amount,
                IsCredit = true,
                TransactionDate = DateTime.UtcNow,
                TransactionType = "credit",
                Status = "Completed"
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Account credit successfully for Account ID {AccountId}", account.AccountId);
            return true;
        }

        private async Task<Account> GetAccount(TransactionRequest request)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == request.AccountId && a.IsActive);
            if (account == null)
            {
                throw new Exception("Account not found.");
            }

            return account;
        }

        public async Task<bool> Debit(TransactionRequest request)
        {
            Account account = await GetAccount(request);

            if (account.Balance < request.Amount)
                throw new Exception("Insufficient funds.");

            account.Balance -= request.Amount;
            var transaction = new Transaction
            {
                AccountId = account.AccountId,
                Amount = request.Amount,
                IsCredit = false,
                TransactionDate = DateTime.UtcNow,
                TransactionType = "debit",
                Status = "Completed"
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Account debit successfully for Account ID {AccountId}", account.AccountId);
            return true;
        }
        public async Task<decimal> GetBalance(int accountId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId && a.IsActive);
            if (account == null)
            {
                throw new Exception("Account not found.");
            }

            return account.Balance;

        }
    }
}
