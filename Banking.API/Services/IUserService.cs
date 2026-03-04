using System.Security.Claims;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllAsync(int page, int pageSize);
    Task<UserDTO> GetByIdAsync(string id);
    Task<bool> UpdateAsync(string id, UserDTO model, ClaimsPrincipal currentUser);
}