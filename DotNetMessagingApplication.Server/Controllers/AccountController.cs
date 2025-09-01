using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Dtos;
using DotNetMessagingApplication.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMessagingApplication.Server.Controllers;

[ApiController]
[Route("api/controllers")]
public class AccountController(IAccountService accountService) : ControllerBase
{
	readonly IAccountService _accountService = accountService;

	[HttpPost("addUser")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult AddNewUser([FromBody] AddNewUserRequest request)
	{
		try
		{
			_accountService.AddUser(request.Username, request.Email, request.Password, request.Pronouns);

			return Ok("Successful sign up!");
		}
		catch (ArgumentException ex)
		{
			return UnprocessableEntity($"Could not sign up: {ex.Message}");
		}
		catch (Exception ex)
		{
			return BadRequest("Unhandled exception: " + ex.Message);
		}
	}

	[HttpPost("updateUser")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
	{
		try
		{
			User user = _accountService.GetDetails(request.oldUsername);
			user.Username = request.Username;
			user.Email = request.Email;
			user.Phone = request.Phone;
			user.Pronouns = request.Pronouns;
			user.Bio = request.Bio;

			_accountService.UpdateDetails(user);
			return Ok("Successful user update!");
		}
        catch (ArgumentException ex)
        {
            return BadRequest("No user with the given user and email could be found: " + ex.Message);
        }
        catch (Exception ex)
		{
			return BadRequest("Unhandled exception: " + ex.Message);
		}
	}

	[HttpPost("details")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult GetAccountDetails([FromBody] AccountDetailsRequest request)
	{
		try
		{
			User user = _accountService.GetDetails(request.EmailOrUsername);

			return Ok(new AccountDetailsResponse
			{
				Username = user.Username,
				Email = user.Email,
				Phone = user.Phone,
				Bio = user.Bio,
				ProfilePhotoLink = user.ProfilePhotoLink,
				Pronouns = user.Pronouns
			});
		}
		catch (InvalidOperationException)
		{
			return NotFound("User not found");
		}
		catch (Exception ex)
		{
			return BadRequest("Unhandled exception: " + ex.Message);
		}
	}
}
