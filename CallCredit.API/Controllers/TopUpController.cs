using CallCredit.API.Interfaces;
using CallCredit.API.Models;
using CallCredit.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CallCredit.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopUpController : ControllerBase
    {
        private readonly ITopUpService _topUpService;
        private readonly ILogger<TopUpController> _logger;

        public TopUpController(ITopUpService topUpService, ILogger<TopUpController> logger)
        {
            _topUpService = topUpService;
            _logger = logger;
        }

        [HttpPost]
        [Route("topup")]
        public async Task<IActionResult> TopUp([FromBody] TopUpRequestModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var success = await _topUpService.PerformTopUp(request);
                _logger.LogInformation("Top-up successful.");                
                if (success)
                {
                    _logger.LogInformation("Top-up successful for User ID {userId} and Beneficiary ID {beneficiaryId}.", request.UserId, request.BeneficiaryId);
                    return Ok(ResponseModel.SuccessResponse("Top-up successful."));
                }
                else
                {
                    _logger.LogWarning("Top-up failed for User ID {userId} and Beneficiary ID {beneficiaryId}.", request.UserId, request.BeneficiaryId);
                    return BadRequest(ResponseModel.ErrorResponse("Top-up failed."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the request: {Error}", ex);
                return StatusCode(500, ResponseModel.ErrorResponse($"An error occurred while processing your request: {ex.Message}"));
            }
        }
        [HttpGet("options")]
        public async Task<IActionResult> GetTopUpOptions()
        {
            var options = await _topUpService.GetTopUpOptions();
            return Ok(options);
        }

    }
}
