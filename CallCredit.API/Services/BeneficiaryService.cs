using CallCredit.API.Interfaces;
using CallCredit.API.Models;
using CallCredit.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CallCredit.API.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly CallCreditContext _context;
        private readonly IRulesService _rulesService;

        public BeneficiaryService(CallCreditContext context, IRulesService rulesService)
        {
            _context = context;
            _rulesService = rulesService;
        } 

        public async Task AddBeneficiary(BeneficiaryRequest request)
        {
            Beneficiary newBeneficiary = new Beneficiary() { Nickname = request.Nickname,UserId=request.UserId,PhoneNumber=request.PhoneNumber };
            await _rulesService.ValidateBeneficiaryAddition(request);
            newBeneficiary.IsActive = true;
            _context.Beneficiaries.Add(newBeneficiary);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Beneficiary>> GetBeneficiariesByUserId(int userId)
        {
            return await _context.Beneficiaries
                            .Where(b => b.UserId == userId )
                            .ToListAsync();

        }

        public async Task RemoveBeneficiary(int beneficiaryId)
        {
            var beneficiary = await _context.Beneficiaries.FindAsync(beneficiaryId);
            if (beneficiary != null)
            {
                _context.Beneficiaries.Remove(beneficiary);
                await _context.SaveChangesAsync();
            }
        }
    }
}
