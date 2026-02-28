using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Banking.Models;

namespace banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanRequestController : ControllerBase
    {
        private readonly ILoanRequestService _loanRequestService;
        private readonly ILogger<LoanRequestController> _logger;

        public LoanRequestController(ILoanRequestService loanRequestService, ILogger<LoanRequestController> logger)
        {
            _loanRequestService = loanRequestService;
            _logger = logger;
        }

        // GET: api/LoanRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanRequest>>> GetLoanRequests()
        {
            try
            {
                _logger.LogInformation("Fetching all loan requests.");
                var loanRequests = await _loanRequestService.GetAllLoanRequestsAsync();
                if (loanRequests == null || !loanRequests.Any())
                {
                    _logger.LogWarning("No loan requests found.");
                    return NotFound("No loan requests found.");
                }

                return Ok(loanRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching loan requests.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/LoanRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanRequest>> GetLoanRequest(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching loan request with ID {id}");
                var loanRequest = await _loanRequestService.GetLoanRequestByIdAsync(id);

                if (loanRequest == null)
                {
                    _logger.LogWarning($"Loan request with ID {id} not found.");
                    return NotFound($"Loan request with ID {id} not found.");
                }

                return Ok(loanRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the loan request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/LoanRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoanRequest(int id, LoanRequestDTO loanRequestDto)
        {
            try
            {
                if (id != loanRequestDto.Id)
                {
                    _logger.LogWarning("Loan request ID mismatch.");
                    return BadRequest("Loan request ID mismatch.");
                }

                var loanRequest = new LoanRequest
                {
                    Id = loanRequestDto.Id,
                    PrincipalAmount = loanRequestDto.PrincipalAmount,
                    DurationInMonths = loanRequestDto.DurationInMonths,
                    BankAccountId = loanRequestDto.BankAccountId,
                    UserId = loanRequestDto.UserId
                };

                await _loanRequestService.UpdateLoanRequestAsync(id, loanRequest);
                _logger.LogInformation($"Loan request with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _loanRequestService.GetLoanRequestByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Loan request with ID {id} not found for update.");
                    return NotFound($"Loan request with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating loan request.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the loan request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/LoanRequests
        [HttpPost]
        public async Task<ActionResult<LoanRequestDTO>> PostLoanRequest(LoanRequestDTO loanRequestDto)
        {
            try
            {
                if (loanRequestDto == null)
                {
                    _logger.LogWarning("Received empty loan request object.");
                    return BadRequest("Loan request data cannot be null.");
                }

                var loanRequest = new LoanRequest
                {
                    PrincipalAmount = loanRequestDto.PrincipalAmount,
                    DurationInMonths = loanRequestDto.DurationInMonths,
                    BankAccountId = loanRequestDto.BankAccountId,
                    UserId = loanRequestDto.UserId
                };
                await _loanRequestService.AddLoanRequestAsync(loanRequest);
                _logger.LogInformation($"Loan request with ID {loanRequest.Id} created.");
                return CreatedAtAction("GetLoanRequest", new { id = loanRequest.Id }, loanRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the loan request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveLoan(LoanApprovalDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Received empty loan approval object.");
                    return BadRequest("Loan approval data cannot be null.");
                }
                var loan = await _loanRequestService.ApproveLoanRequestAsync(dto);
                if (loan == null)
                {
                    _logger.LogInformation($"Loan request with ID {dto.LoanRequestId} rejected.");
                    return Ok("Loan rejected successfully");
                }
                _logger.LogInformation($"Loan request with ID {dto.LoanRequestId} approved.");
                return Ok(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving the loan request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/LoanRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoanRequest(int id)
        {
            try
            {
                var loanRequest = await _loanRequestService.GetLoanRequestByIdAsync(id);
                if (loanRequest == null)
                {
                    _logger.LogWarning($"Loan request with ID {id} not found.");
                    return NotFound($"Loan request with ID {id} not found.");
                }

                await _loanRequestService.DeleteLoanRequestAsync(id);
                _logger.LogInformation($"Loan request with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the loan request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
