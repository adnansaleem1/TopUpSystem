using CallCredit.API.Models;
using CallCredit.Data.Entities;

namespace CallCredit.API.Interfaces
{
    public interface IRulesService
    {
        Task<bool> ValidateBeneficiaryAddition(BeneficiaryRequest request);
        Task<bool> ValidateTopUp(TopUpValidationModel request);
        bool ValidateBalnaceForTopUp(decimal amount, int chargeAmount, decimal balance);
        int GetChargeAmount();
    }
}
