using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Banking.Models;
using Microsoft.AspNetCore.Authorization;

namespace banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly ILogger<LoanController> _logger;

        public LoanController(ILoanService loanService, ILogger<LoanController> logger)
        {
            _loanService = loanService;
            _logger = logger;
        }

        // GET: api/Loans
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            try
            {
                _logger.LogInformation("Fetching all loans.");
                var loans = await _loanService.GetAllLoansAsync();
                if (loans == null || !loans.Any())
                {
                    _logger.LogWarning("No loans found.");
                    return NotFound("No loans found.");
                }
                var loanRepsonses = loans.Select(loan => new LoanResponseDTO
                {
                    Id = loan.Id,
                    PrincipalAmount = loan.PrincipalAmount,
                    DurationInMonths = loan.DurationInMonths,
                    BankAccountId = loan.BankAccountId,
                    UserId = loan.UserId,
                }).ToList();
                return Ok(loanRepsonses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching loans.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Loan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching loan with ID {id}");
                var loan = await _loanService.GetLoanByIdAsync(id);

                if (loan == null)
                {
                    _logger.LogWarning($"Loan with ID {id} not found.");
                    return NotFound($"Loan with ID {id} not found.");
                }
                var loanResponse = new LoanResponseDTO
                {
                    PrincipalAmount = loan.PrincipalAmount,
                    DurationInMonths = loan.DurationInMonths,
                    BankAccountId = loan.BankAccountId,
                    UserId = loan.UserId,
                };
                return Ok(loanResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the loan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Loans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(int id, LoanDTO loanDto)
        {
            try
            {
                if (id != loanDto.Id)
                {
                    _logger.LogWarning("Loan ID mismatch.");
                    return BadRequest("Loan ID mismatch.");
                }

                var loan = new Loan
                {
                    Id = loanDto.Id,
                    DurationInMonths = loanDto.DurationInMonths,
                    BankAccountId = loanDto.BankAccountId,
                    UserId = loanDto.UserId
                };

                await _loanService.UpdateLoanAsync(id, loan);
                _logger.LogInformation($"Loan with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _loanService.GetLoanByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Loan with ID {id} not found for update.");
                    return NotFound($"Loan with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating loan.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the loan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Loans
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<LoanDTO>> PostLoan(LoanDTO loanDto)
        {
            try
            {
                if (loanDto == null)
                {
                    _logger.LogWarning("Received empty loan object.");
                    return BadRequest("Loan data cannot be null.");
                }

                var loan = new Loan
                {
                    PrincipalAmount = loanDto.PrincipalAmount,
                    DurationInMonths = loanDto.DurationInMonths,
                    BankAccountId = loanDto.BankAccountId,
                    UserId = loanDto.UserId
                };
                await _loanService.AddLoanAsync(loan);
                _logger.LogInformation($"Loan with ID {loan.Id} created.");
                return CreatedAtAction("GetLoan", new { id = loan.Id }, loan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the loan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Loans/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                var loan = await _loanService.GetLoanByIdAsync(id);
                if (loan == null)
                {
                    _logger.LogWarning($"Loan with ID {id} not found.");
                    return NotFound($"Loan with ID {id} not found.");
                }

                await _loanService.DeleteLoanAsync(id);
                _logger.LogInformation($"Loan with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the loan.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
