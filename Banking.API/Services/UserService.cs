using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync(int page, int pageSize)
    {
        return _userManager.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName
            });
    }

    // public async Task<UserDTO> GetByIdAsync(string id)
    // {
    //     var currentUserId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
    //     Console.WriteLine($"CurrentUser: {currentUserId}");

    //     if (currentUserId != id)
    //         throw new UnauthorizedAccessException();
    //     var user = await _userManager.FindByIdAsync(id);

    //     if (user == null)
    //         return null;

    //     return new UserDTO
    //     {
    //         Id = user.Id,
    //         Email = user.Email,
    //         FirstName = user.FirstName,
    //         LastName = user.LastName
    //     };
    // }

public async Task<UserDTO> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return null;

        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PasswordHash = user.PasswordHash,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address
        };
    }


    public async Task<bool> UpdateAsync(string id, UserDTO model, ClaimsPrincipal currentUser)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return false;

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.Address = model.Address;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }
}