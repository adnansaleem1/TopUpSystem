using CallCredit.API.Models;
using CallCredit.Data.Entities;

namespace CallCredit.API.Interfaces
{
    public interface IBeneficiaryService
    {
        Task AddBeneficiary(BeneficiaryRequest request);
        Task<List<Beneficiary>> GetBeneficiariesByUserId(int userId);
        Task RemoveBeneficiary(int beneficiaryId);
    }
}
