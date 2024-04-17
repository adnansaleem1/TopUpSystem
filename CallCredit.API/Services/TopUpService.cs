using CallCredit.API.Controllers;
using CallCredit.API.Interfaces;
using CallCredit.API.Models;
using CallCredit.API.Common;
using CallCredit.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CallCredit.API.Services
{
    public class TopUpService : ITopUpService
    {
        private readonly CallCreditContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IRulesService _rulesService;
        private readonly string _externalServiceBaseUrl;
        private readonly ExternalServiceSettings _settings;
        private readonly ILogger<TopUpService> _logger;

        public TopUpService(CallCreditContext context, IHttpClientFactory httpClientFactory, IRulesService rulesService, IOptions<ExternalServiceSettings> settings, ILogger<TopUpService> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _rulesService = rulesService;
            _settings = settings.Value;
            _externalServiceBaseUrl = _settings.BaseUrl;
            _logger = logger;
        }

        public async Task<List<TopUpOption>> GetTopUpOptions()
        {
            return await _context.TopUpOptions.ToListAsync();
        }

        public async Task<bool> PerformTopUp(TopUpRequestModel request)
        {                            
            
            bool isDebited = false;
            bool isBeneficiaryCredited = false;
            bool isVendorCredited = false;
            TopUpOption? topUpOption = await GetTopUpOptions(request);
            using var transaction = await _context.Database.BeginTransactionAsync();            
            try
            {                       
                await _rulesService.ValidateTopUp(new TopUpValidationModel(request, topUpOption));
                isDebited = await Debit(AccountName.User, topUpOption.Amount, _rulesService.GetChargeAmount());
                // topUp                
                if (isDebited)
                {
                    isBeneficiaryCredited = await Credit(AccountName.Beneficiary, topUpOption.Amount);
                    isVendorCredited = await Credit(AccountName.TopUpVendor, topUpOption.Amount);
                    await Credit(AccountName.Company, topUpOption.Amount + _rulesService.GetChargeAmount());

                    var topUpTransaction = new TopUpTransaction
                    {
                        BeneficiaryId = request.BeneficiaryId,
                        Amount = topUpOption.Amount,
                        TransactionDate = DateTime.Now,
                        UserId=request.UserId,
                        Charge = _rulesService.GetChargeAmount()
                    };
                    // TODO:  call a third-party system to perform a top-up for a beneficiary number
                    _context.TopUpTransactions.Add(topUpTransaction);
                    await _context.SaveChangesAsync();

                    // TODO: Implement handling for distributed systems due to external service. Consider using a ServiceBus or equivalent system to ensure advanced messaging and transactions.
                    await transaction.CommitAsync();                    
                    return true;
                }
                _logger.LogWarning("Top-up failed for User ID {userId} and Beneficiary ID {beneficiaryId}.", request.UserId, request.BeneficiaryId);
                return false;
            }
            catch (Exception ex)
            {
                await ReverseEntries(topUpOption, isDebited, isBeneficiaryCredited, isVendorCredited);
                await transaction.RollbackAsync();
                _logger.LogError("An error occurred while processing the request: {Error}", ex);
                throw;
            }
        }

        private async Task<TopUpOption?> GetTopUpOptions(TopUpRequestModel request)
        {
            try
            {
                return await _context.TopUpOptions.FindAsync(request.TopUpOptionId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in GetTopUpOptions: {Error}", ex);
                throw new Exception($"TopUpOption with ID {request.TopUpOptionId} not found.");
            }         

        }
        private async Task<bool> Debit(AccountName account,decimal amount, int chargeAmount=0)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var balanceResponse = await client.GetAsync($"{_externalServiceBaseUrl}/{(int)account}/Balance");
                var balance = Convert.ToDecimal(await balanceResponse.Content.ReadAsStringAsync());
                
                _rulesService.ValidateBalnaceForTopUp(amount, chargeAmount, balance);

                var response = await client.PostAsJsonAsync($"{_externalServiceBaseUrl}/debit", new
                {                   
                    AccountId=(int)account,
                    Amount = amount + chargeAmount,
                    IsCredit = false,
                    TransactionType = "Top-Up Charge"
                });

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException("Failed to debit account");
            }
            catch (Exception)
            {
                return false;
            }
                      
            return true;
        }
        private async Task<bool> Credit(AccountName account, decimal amount)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();                         

                var response = await client.PostAsJsonAsync($"{_externalServiceBaseUrl}/credit", new
                {
                    AccountId= (int)account,
                    Amount =amount,
                    IsCredit = true,
                    TransactionType = "Top-Up Charge"
                });

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException("Failed to credit account");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        private async Task ReverseEntries(TopUpOption? topUpOption, bool isDebited, bool isBeneficiaryCredited, bool isVendorCredited)
        {
            try
            {
                if (topUpOption == null)
                {
                    _logger.LogError("An error occurred while processing reverse entries: {Error}", "TopUpOption is not available.");
                }
                if (isVendorCredited)
                {
                    await Debit(AccountName.TopUpVendor, topUpOption.Amount);

                }
                if (isBeneficiaryCredited)
                {
                    await Debit(AccountName.Beneficiary, topUpOption.Amount);
                }
                if (isDebited)
                {
                    await Credit(AccountName.User, topUpOption.Amount + _rulesService.GetChargeAmount());

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing reverse entries: {Error}", ex);
            }
        }

    }
}
