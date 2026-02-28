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
    public class CardRequestController : ControllerBase
    {
        private readonly ICardRequestService _cardRequestService;
        private readonly IBankAccountService _bankAccountService;
        private readonly ILogger<CardRequestController> _logger;

        public CardRequestController(ICardRequestService cardRequestService, IBankAccountService bankAccountService, ILogger<CardRequestController> logger)
        {
            _cardRequestService = cardRequestService;
            _bankAccountService = bankAccountService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        // GET: api/CardRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardRequest>>> GetCardRequests()
        {
            try
            {
                _logger.LogInformation("Fetching all card requests.");
                var cardRequests = await _cardRequestService.GetAllCardRequestsAsync();
                if (cardRequests == null || !cardRequests.Any())
                {
                    _logger.LogWarning("No card requests found.");
                    return NotFound("No card requests found.");
                }
                return Ok(cardRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching card requests.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/CardRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CardRequest>> GetCardRequest(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching card request with ID {id}");
                var cardRequest = await _cardRequestService.GetCardRequestByIdAsync(id);

                if (cardRequest == null)
                {
                    _logger.LogWarning($"Card request with ID {id} not found.");
                    return NotFound($"Card request with ID {id} not found.");
                }

                return Ok(cardRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the card request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/CardRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardRequest(int id, CardRequestDTO cardRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)  
                    return BadRequest(ModelState);
                if (id != cardRequestDTO.Id)
                {
                    _logger.LogWarning("Card request ID mismatch.");
                    return BadRequest("Card request ID mismatch.");
                }

                var cardRequest = new CardRequest
                {
                    Id = cardRequestDTO.Id,
                    CardType = cardRequestDTO.CardType,
                    CardBrand = cardRequestDTO.CardBrand,
                    AccountId = cardRequestDTO.AccountId,
                    UserId = cardRequestDTO.UserId
                };

                await _cardRequestService.UpdateCardRequestAsync(id, cardRequest);
                _logger.LogInformation($"Card request with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _cardRequestService.GetCardRequestByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Card request with ID {id} not found for update.");
                    return NotFound($"Card request with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating card request.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the card request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/CardRequests
        [HttpPost]
        public async Task<ActionResult<CardRequest>> PostCardRequest(CardRequestDTO cardRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)  
                    return BadRequest(ModelState);
                var bankAccount = await _bankAccountService.GetBankAccountByIdAsync(cardRequestDTO.AccountId);

                if (bankAccount == null || bankAccount.AccountStatus != AccountStatus.Active)
                    throw new Exception("User must have an active bank account to request a card");
                if (cardRequestDTO == null)
                {
                    _logger.LogWarning("Received empty card request object.");
                    return BadRequest("Card request data cannot be null.");
                }

                var cardRequest = new CardRequest
                {
                    CardType = cardRequestDTO.CardType,
                    CardBrand = cardRequestDTO.CardBrand,
                    AccountId = cardRequestDTO.AccountId,
                    UserId = cardRequestDTO.UserId
                };

                await _cardRequestService.AddCardRequestAsync(cardRequest);
                _logger.LogInformation($"Card request with ID {cardRequest.Id} created.");
                return CreatedAtAction("GetCardRequest", new { id = cardRequest.Id }, cardRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the card request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("approve")]
        public async Task<IActionResult> ApproveCard(CardApprovalDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)  
                    return BadRequest(ModelState);
                if (dto == null)
                {
                    _logger.LogWarning("Received empty card approval object.");
                    return BadRequest("Card approval data cannot be null.");
                }
                var card = await _cardRequestService.ApproveCardRequestAsync(dto);
                if (card == null)
                {
                    _logger.LogInformation($"Card request with ID {dto.CardRequestId} rejected.");
                    return Ok("Card rejected successfully");
                }
                _logger.LogInformation($"Card request with ID {dto.CardRequestId} approved.");
                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving the card request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/CardRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardRequest(int id)
        {
            try
            {
                var cardRequest = await _cardRequestService.GetCardRequestByIdAsync(id);
                if (cardRequest == null)
                {
                    _logger.LogWarning($"Card request with ID {id} not found.");
                    return NotFound($"Card request with ID {id} not found.");
                }

                await _cardRequestService.DeleteCardRequestAsync(id);
                _logger.LogInformation($"Card request with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the card request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
