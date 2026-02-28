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
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        // GET: api/Transaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            try
            {
                _logger.LogInformation("Fetching all transactions.");
                var transactions = await _transactionService.GetAllTransactionsAsync();
                if (transactions == null || !transactions.Any())
                {
                    _logger.LogWarning("No transactions found.");
                    return NotFound("No transactions found.");
                }
                var transactionResponses = transactions.Select(t => new TransactionResponseDTO
                {
                    TransactionId = t.TransactionId,
                    AccountId = t.AccountId,
                    Amount = t.Amount,
                    TransactionType = t.TransactionType,
                    Description = t.Description,
                    Reference = t.Reference,
                    Timestamp = t.Timestamp
                }).ToList();
                return Ok(transactionResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching transactions.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching transaction with ID {id}");
                var transaction = await _transactionService.GetTransactionByIdAsync(id);

                if (transaction == null)
                {
                    _logger.LogWarning($"Transaction with ID {id} not found.");
                    return NotFound($"Transaction with ID {id} not found.");
                }
                var transactionResponse = new TransactionResponseDTO
                {
                    TransactionId = transaction.TransactionId,
                    AccountId = transaction.AccountId,
                    Amount = transaction.Amount,
                    TransactionType = transaction.TransactionType,
                    Description = transaction.Description,
                    Reference = transaction.Reference,
                    Timestamp = transaction.Timestamp
                };
                return Ok(transactionResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the transaction.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Transaction/Deposit
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Deposit")]
        public async Task<ActionResult<Transaction>> DepositTransaction(TransactionDTO transactionDTO)
        {
            try
            {
                if (!ModelState.IsValid)  
                    return BadRequest(ModelState);
                if (transactionDTO == null)
                {
                    _logger.LogWarning("Received empty transaction object.");
                    return BadRequest("Transaction data cannot be null.");
                }

                var transaction = new Transaction
                {
                    Amount = transactionDTO.Amount,
                    Status = transactionDTO.Status,
                    TransactionType = transactionDTO.TransactionType,
                    AccountId = transactionDTO.AccountId
                };
                await _transactionService.DepositAsync(transaction.AccountId, transaction.Amount);
                _logger.LogInformation($"Deposit transaction with ID {transaction.TransactionId} created.");
                return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the transaction.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Transaction/Withdraw
        [HttpPost("Withdraw")]
        public async Task<ActionResult<Transaction>> WithdrawTransaction(TransactionDTO transactionDTO)
        {
            try
            {
                if (!ModelState.IsValid)  
                    return BadRequest(ModelState);
                if (transactionDTO == null)
                {
                    _logger.LogWarning("Received empty transaction object.");
                    return BadRequest("Transaction data cannot be null.");
                }

                var transaction = new Transaction
                {
                    Amount = transactionDTO.Amount,
                    Status = transactionDTO.Status,
                    TransactionType = transactionDTO.TransactionType,
                    AccountId = transactionDTO.AccountId
                };
                await _transactionService.WithdrawAsync(transaction.AccountId, transaction.Amount);
                _logger.LogInformation($"Withdrawal transaction with ID {transaction.TransactionId} created.");
                return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the transaction.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Transaction/transfer
        [HttpPost("Transfer")]
        public async Task<ActionResult<Transaction>> TransferTransaction(TransactionDTO transactionDTO)
        {
            try
            {
                if (!ModelState.IsValid)  
                    return BadRequest(ModelState);
                if (transactionDTO == null)
                {
                    _logger.LogWarning("Received empty transaction object.");
                    return BadRequest("Transaction data cannot be null.");
                }

                var transaction = new TransactionDTO
                {
                    Amount = transactionDTO.Amount,
                    AccountId = transactionDTO.AccountId,
                    RecipientAccountNumber = transactionDTO.RecipientAccountNumber
                };
                await _transactionService.TransferAsync(transaction.AccountId, transaction.RecipientAccountNumber, transaction.Amount);
                _logger.LogInformation($"Transfer transaction with ID {transaction.TransactionId} created.");
                return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the transaction.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
