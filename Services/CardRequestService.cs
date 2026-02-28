using Banking.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

public class CardRequestService : ICardRequestService
{
    private readonly ICardRequestRepository _cardRequestRepository;
    private readonly ICardService _cardService;
    private readonly IBankAccountService _bankAccountService;

    public CardRequestService(ICardRequestRepository cardRequestRepository, ICardService cardService, IBankAccountService bankAccountService)
    {
        _cardRequestRepository = cardRequestRepository;
        _cardService = cardService;
        _bankAccountService = bankAccountService;
    }

    public async Task<CardRequest> AddCardRequestAsync(CardRequest cardRequest)
    {
        await _cardRequestRepository.AddAsync(cardRequest);
        return cardRequest;
    }

    public async Task<Card> ApproveCardRequestAsync(CardApprovalDTO dto)
    {
        var request = await _cardRequestRepository.GetByIdAsync(dto.CardRequestId);
        if (request == null) throw new Exception("Card request not found");
        if (request.Status != CardRequestStatus.Pending) throw new Exception("Already processed");

        var bankAccount = await _bankAccountService.GetBankAccountByIdAsync(request.AccountId);

                if (bankAccount == null || bankAccount.AccountStatus != AccountStatus.Active)
                    throw new Exception("User must have an active bank account to request a card");

        request.Status = dto.IsApproved == CardRequestStatus.Approved ? CardRequestStatus.Approved : CardRequestStatus.Rejected;
        request.ReviewedAt = DateTime.UtcNow;

        if (dto.IsApproved != CardRequestStatus.Approved)
        {
            await _cardRequestRepository.SaveChangesAsync();
            return null;
        }

        // Create actual Card
        var cardNumber = GenerateCardNumber(request.CardBrand);
        var cvv = GenerateCVV();
        var pin = GeneratePin();

        var card = new Card
        {
            CardNumberHash = Hash(cardNumber),
            CVVHash = Hash(cvv),
            PinHash = Hash(pin),
            Last4Digits = cardNumber[^4..],

            CardType = request.CardType,
            CardBrand = request.CardBrand,
            ExpiryDate = DateTime.UtcNow.AddYears(4),

            CreatedAt = DateTime.UtcNow,
            UserId = request.UserId,
            BankAccountId = request.AccountId
        };



        await _cardService.AddCardAsync(card);
        await _cardRequestRepository.SaveChangesAsync();
        return card;
        // return new CardResponseDTO
        // {
        //     Id = card.Id,
        //     MaskedNumber = $"**** **** **** {card.Last4Digits}",
        //     CardType = card.CardType,
        //     CardBrand = card.CardBrand,
        //     ExpiryDate = card.ExpiryDate
        //     // In real life, you would NOT return CVV or PIN
        // };
    }
    public async Task<CardRequest> GetCardRequestByIdAsync(int cardRequestId)
    {
        return await _cardRequestRepository.GetByIdAsync(cardRequestId);
    }

    public async Task<IEnumerable<CardRequest>> GetAllCardRequestsAsync()
    {
        return await _cardRequestRepository.GetAllAsync();
    }

    public async Task<CardRequest> UpdateCardRequestAsync(int cardRequestId, CardRequest cardRequest)
    {
        await _cardRequestRepository.UpdateAsync(cardRequest);
        return cardRequest;
    }

    public async Task<bool> DeleteCardRequestAsync(int cardRequestId)
    {
        var cardRequest = await _cardRequestRepository.GetByIdAsync(cardRequestId);
        if (cardRequest == null) return false;

        await _cardRequestRepository.DeleteAsync(cardRequestId);
        return true;
    }
    private string GenerateCardNumber(CardBrand brand)
    {
        var random = new Random();

        string prefix = brand switch
        {
            CardBrand.Visa => "4",
            CardBrand.MasterCard => "5",
            _ => "9"
        };

        string number = prefix;

        for (int i = 0; i < 15; i++)
            number += random.Next(0, 10).ToString();

        return number;
    }

    private string GenerateCVV()
    {
        var random = new Random();
        return random.Next(100, 999).ToString();
    }

    private string GeneratePin()
    {
        var random = new Random();
        return random.Next(1000, 9999).ToString();
    }

    private string Hash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}