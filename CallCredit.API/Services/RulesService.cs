using Azure.Core;
using CallCredit.API.Interfaces;
using CallCredit.API.Models;
using CallCredit.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CallCredit.API.Services
{
    public class RulesService : RulesServiceBase, IRulesService
    {
        private readonly CallCreditContext _context;      

        public RulesService(CallCreditContext context)
        {
            _context = context;
            
        }
        public async Task<bool> ValidateBeneficiaryAddition(BeneficiaryRequest request)
        {
            List<string> validationErrors = new List<string>();

            if (string.IsNullOrEmpty(request.Nickname) || request.Nickname.Length > MaxNickNameLength)
            {
                validationErrors.Add($"The nickname must be {MaxNickNameLength} characters or fewer.");
            }

            var count = await _context.Beneficiaries.CountAsync(b => b.UserId == request.UserId && b.IsActive);
            if (count >= MaxBeneficiary)
            {
                validationErrors.Add($"Cannot add more than {MaxBeneficiary} beneficiaries.");
            }

            if (validationErrors.Any())
            {
                throw new InvalidOperationException(string.Join(' ', validationErrors));
            }
            return true;
        }
      
        public async Task<bool> ValidateTopUp(TopUpValidationModel request)
        {
            
            List<string> validationErrors = new List<string>();
            var user = await _context.Users.FindAsync(request.UserId);
            var beneficiary = await _context.Beneficiaries
                                .Where(b => b.BeneficiaryId == request.BeneficiaryId && b.UserId==request.UserId && b.IsActive)
                                .FirstOrDefaultAsync();
            var topUpOption = request.TopUpOption;

            if (user == null)
                validationErrors.Add("User not found.");
            if (beneficiary == null)
                validationErrors.Add("Beneficiary not found or not active.");
            if (topUpOption == null)
                validationErrors.Add("Invalid top-up option.");

            if (user!=null && beneficiary != null && topUpOption != null)
            {
                var totalTopUpThisMonth = await _context.TopUpTransactions
                    .Where(t => t.UserId == request.UserId && t.TransactionDate.Month == DateTime.UtcNow.Month)
                    .SumAsync(t => t.Amount);


                if (totalTopUpThisMonth + topUpOption.Amount > MaxAmountMonth)
                {
                    var remainingLimit = MaxAmountMonth - totalTopUpThisMonth;
                    validationErrors.Add($"Exceeds monthly top-up limit of AED {MaxAmountMonth}. Remaining monthly top-ups are AED {remainingLimit}.");
                }

                var beneficiaryTopUpThisMonth = await _context.TopUpTransactions
                    .Where(t => t.BeneficiaryId == request.BeneficiaryId && t.TransactionDate.Month == DateTime.UtcNow.Month)
                    .SumAsync(t => t.Amount);

                var individualLimit = user.IsVerified ? VerifiedUserMaxAmount : NotVerifiedUserMaxAmount;
                if (beneficiaryTopUpThisMonth + topUpOption.Amount > individualLimit)
                {
                    validationErrors.Add($"Exceeds monthly top-up limit of AED {individualLimit} for {(user.IsVerified ? "verified" : "unverified")} users.");
                }
            }

            if (validationErrors.Count != 0)
            {
                throw new InvalidOperationException(string.Join(' ', validationErrors));
            }

            return true;
        }
        public bool ValidateBalnaceForTopUp(decimal amount, int chargeAmount, decimal balance)
        {
            if (balance < amount + chargeAmount) 
                throw new Exception("Insufficient funds.");

            return true;
        }

        public int GetChargeAmount()
        {
            return ChargeAmount;
        }
    }

}
