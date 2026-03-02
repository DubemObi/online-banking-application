using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Banking.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;

namespace banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users.");
                var users = await _userService.GetAllAsync(1, 100);
                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found.");
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
        {
            try
            {
                _logger.LogInformation($"Fetching user with ID {id}");
                var user = await _userService.GetByIdAsync(id, User);

                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound($"User with ID {id} not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserDTO userDTO)
        {
            try
            {
                if (!ModelState.IsValid)  
                    return BadRequest(ModelState);
                if (id != userDTO.Id)
                {
                    _logger.LogWarning("User ID mismatch.");
                    return BadRequest("User ID mismatch.");
                }

                var isUpdated = await _userService.UpdateAsync(id, userDTO, User);
                if (!isUpdated)
                {
                    _logger.LogWarning($"Failed to update user with ID {id}.");
                    return NoContent();
                }
                return Ok("User updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _userService.GetByIdAsync(id, User) == null)
                {
                    _logger.LogWarning($"User with ID {id} not found for update.");
                    return NotFound($"User with ID {id} not found.");
                }
                else
                {
                    _logger.LogError("Error updating user.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


        // [Authorize(Roles = "Admin")]
        // // DELETE: api/User/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteUser(string id)
        // {
        //     var user = await _userService.GetByIdAsync(id, User);
        //     if (user == null)
        //     {
        //             _logger.LogWarning($"User with ID {id} not found for deletion.");
        //         return NotFound();
        //     }

        //     _userService.DeleteUser(user);
        //     await _userService.SaveChangesAsync();

        //     return NoContent();
        // }
    }
        
}
