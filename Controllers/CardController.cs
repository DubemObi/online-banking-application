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
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly ILogger<CardController> _logger;

        public CardController(ICardService cardService, ILogger<CardController> logger)
        {
            _cardService = cardService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        // GET: api/Card
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            try
            {
                _logger.LogInformation("Fetching all cards.");
                var cards = await _cardService.GetAllCardsAsync();
                if (cards == null || !cards.Any())
                {
                    _logger.LogWarning("No cards found.");
                    return NotFound("No cards found.");
                }
                var cardResponses = cards.Select(card => new CardResponseDTO
                {
                    Id = card.Id,
                    CardNumberHash = card.CardNumberHash,
                    Last4Digits = card.Last4Digits,
                    CardType = card.CardType,
                    CardBrand = card.CardBrand,
                    ExpiryDate = card.ExpiryDate,
                    CVVHash = card.CVVHash,
                    PinHash = card.PinHash,
                    UserId = card.UserId,
                    BankAccountId = card.BankAccountId
                }).ToList();
                return Ok(cardResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching cards.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Card/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching card with ID {id}");
                var card = await _cardService.GetCardByIdAsync(id);

                if (card == null)
                {
                    _logger.LogWarning($"Card with ID {id} not found.");
                    return NotFound($"Card with ID {id} not found.");
                }

                return Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the card.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [Authorize(Roles = "Admin")]
        // PUT: api/Card/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCard(int id, CardDTO cardDTO)
        {
            try
            {
                if (id != cardDTO.Id)
                {
                    _logger.LogWarning("Card ID mismatch.");
                    return BadRequest("Card ID mismatch.");
                }

                var card = new Card
                {
                    CardNumberHash = cardDTO.CardNumber,
                    Last4Digits = cardDTO.Last4Digits,
                    CardType = cardDTO.CardType,
                    CardBrand = cardDTO.CardBrand,
                    ExpiryDate = cardDTO.ExpiryDate,
                    CVVHash = cardDTO.CVV,
                    PinHash = cardDTO.Pin,
                    UserId = cardDTO.UserId,
                    BankAccountId = cardDTO.BankAccountId
                };

                await _cardService.UpdateCardAsync(id, card);
                _logger.LogInformation($"Card with ID {id} updated.");
                return NoContent();
            }
            
            catch (DbUpdateConcurrencyException)
            {
                if (await _cardService.GetCardByIdAsync(id) == null)
                {
                    _logger.LogWarning($"Card with ID {id} not found for update.");
                    return NotFound($"Card with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating card.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the card.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        // POST: api/Card
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Card>> PostCard(CardDTO cardDTO)
        {
            try
            {
                if (cardDTO == null)
                {
                    _logger.LogWarning("Received empty card DTO.");
                    return BadRequest("Card data cannot be null.");
                }

                var card = new Card
                {
                    CardNumberHash = cardDTO.CardNumber,
                    Last4Digits = cardDTO.Last4Digits,
                    CardType = cardDTO.CardType,
                    CardBrand = cardDTO.CardBrand,
                    ExpiryDate = cardDTO.ExpiryDate,
                    CVVHash = cardDTO.CVV,
                    PinHash = cardDTO.Pin,
                    UserId = cardDTO.UserId,
                    BankAccountId = cardDTO.BankAccountId
                };

                var createdCard = await _cardService.AddCardAsync(card);
                _logger.LogInformation($"Card with ID {createdCard.Id} created.");
                return CreatedAtAction("GetCard", new { id = createdCard.Id }, createdCard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the card.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Card/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            try
            {
                var card = await _cardService.GetCardByIdAsync(id);
                if (card == null)
                {
                    _logger.LogWarning($"Card with ID {id} not found.");
                    return NotFound($"Card with ID {id} not found.");
                }

                await _cardService.DeleteCardAsync(id);
                _logger.LogInformation($"Card with ID {id} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the card.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
