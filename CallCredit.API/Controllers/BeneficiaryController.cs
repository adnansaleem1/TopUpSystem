using CallCredit.API.Interfaces;
using CallCredit.API.Models;
using CallCredit.API.Services;
using CallCredit.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CallCredit.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BeneficiaryController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly ILogger<BeneficiaryController> _logger;

        public BeneficiaryController(ILogger<BeneficiaryController> logger,IBeneficiaryService beneficiaryService)
        {
            _logger = logger;
            _beneficiaryService = beneficiaryService;
            
        }

        [HttpPost]
        public async Task<IActionResult> AddBeneficiary([FromBody] BeneficiaryRequest request)
        {
            try
            {
                await _beneficiaryService.AddBeneficiary(request);
                _logger.LogInformation("Beneficiary added successfully.");
                return Ok(ResponseModel.SuccessResponse("Beneficiary added successfully."));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("An error occurred while processing the request: {Error}", ex);
                return BadRequest(ResponseModel.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("An error occurred while processing the request: {Error}", ex);
                return BadRequest(ResponseModel.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the request: {Error}", ex);
                return BadRequest(ResponseModel.ErrorResponse($"An error occurred while adding the beneficiary: {ex.Message}"));
            }
        }
  
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBeneficiariesByUser(int userId)
        {
            try
            {
                var beneficiaries = await _beneficiaryService.GetBeneficiariesByUserId(userId);
                if (beneficiaries == null || !beneficiaries.Any())
                    return NotFound("No beneficiaries found.");

                return Ok(beneficiaries);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the request: {Error}", ex);
                return StatusCode(500, "An internal error occurred. Please try again later.");
            }
        }
    }
}
