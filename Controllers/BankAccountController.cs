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
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly ILogger<BankAccountController> _logger;

        public BankAccountController(IBankAccountService bankAccountService, ILogger<BankAccountController> logger)
        {
            _bankAccountService = bankAccountService;
            _logger = logger;
        }

        // GET: api/BankAccount
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccounts()
        {
            try
            {
            _logger.LogInformation("Retrieving all bank accounts.");
            var bankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
            if (bankAccounts == null || !bankAccounts.Any())
            {
                _logger.LogWarning("No bank accounts found.");
                return NotFound("No bank accounts found.");
            }
            return Ok(bankAccounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving bank accounts.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/BankAccount/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccount>> GetBankAccount(int id)
        {
            try
            {_logger.LogInformation($"Retrieving bank account with ID {id}.");
            var bankAccount = await _bankAccountService.GetBankAccountByIdAsync(id);
            if (bankAccount == null)
            {
                _logger.LogWarning($"Bank account with ID {id} not found.");
                return NotFound($"Bank account with ID {id} not found.");
            }
            
            return Ok(bankAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving bank account with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/BankAccount/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankAccount(int id, BankAccount bankAccount)
        {
            try
            {
                if (id != bankAccount.AccountId)
                {
                    _logger.LogWarning("Bank Account ID mismatch.");
                    return BadRequest("Bank Account ID mismatch.");
                }

                await _bankAccountService.UpdateBankAccountAsync(id, bankAccount);
                _logger.LogInformation($"Bank Account with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _bankAccountService.GetBankAccountByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Bank Account with ID {id} not found for update.");
                    return NotFound($"Bank Account with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating bank account.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the bank account.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/BankAccount
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BankAccount>> PostBankAccount(BankAccount bankAccount)
        {
           try
            {
                if (bankAccount == null)
                {
                    _logger.LogWarning("Received empty bank account object.");
                    return BadRequest("Bank Account data cannot be null.");
                }

                await _bankAccountService.AddBankAccountAsync(bankAccount);
                _logger.LogInformation($"Bank Account with ID {bankAccount.AccountId} created.");
                return CreatedAtAction("GetBankAccount", new { id = bankAccount.AccountId }, bankAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the bank account.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
            }

        // DELETE: api/BankAccount/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBankAccount(int id)
        {
            try
            {
                var bankAccount = await _bankAccountService.GetBankAccountByIdAsync(id);
                if (bankAccount == null)
                {
                    _logger.LogWarning($"Bank Account with ID {id} not found.");
                    return NotFound($"Bank Account with ID {id} not found.");
                }

                await _bankAccountService.DeleteBankAccountAsync(id);
                _logger.LogInformation($"Bank Account with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the bank account.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
