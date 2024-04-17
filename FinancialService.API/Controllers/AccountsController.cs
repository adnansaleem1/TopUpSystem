using CallCredit.API.Interfaces;
using FinancialService.Data;
using FinancialService.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly FinancialContext _context;
        private readonly IAccountsService accountsService;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(FinancialContext context, IAccountsService accountsService, ILogger<AccountsController> logger)
        {
            _context = context;
            this.accountsService = accountsService;
            _logger = logger;
        }

        [HttpGet("{accountId}/Balance")]
        public async Task<IActionResult> GetBalance(int accountId)
        {
            decimal balance = await accountsService.GetBalance(accountId);
            return Ok(balance);
        }

        [HttpPost("debit")]
        public async Task<IActionResult> Debit([FromBody] TransactionRequest request)
        {
            try
            {
                await accountsService.Debit(request);
                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the request: {Error}", ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("credit")]
        public async Task<IActionResult> Credit([FromBody] TransactionRequest request)
        {
            try
            {
                await accountsService.Credit(request);
                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the request: {Error}", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
