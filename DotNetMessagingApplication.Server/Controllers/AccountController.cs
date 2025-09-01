using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Dtos;
using DotNetMessagingApplication.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
            var validationMessage = ValidateAccountDetails(request.Username, request.Password, request.Email, request.Pronouns);
            if (validationMessage != string.Empty)
            {
                return BadRequest(validationMessage);
            }
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
            var validationMessage = ValidateAccountDetails(request.Username, user.Password, request.Email, request.Pronouns, request.Phone);
            if (validationMessage != string.Empty)
            {
                return BadRequest(validationMessage);
            }

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
				Id = user.Id,
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

	private string ValidateAccountDetails(string username, string password, string email, string pronouns, string? phone=null)
	{
		var validateCredsExp = new Regex("^[a-zA-z0-9_.-]+$");
		var validateEmailExp = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$");

		if (!validateCredsExp.IsMatch(password)) {
			return "Password contains invalid characters.";
		}

        if (!validateCredsExp.IsMatch(username))
        {
            return "Username contains invalid characters";
        }

		Console.WriteLine(email);
		Console.WriteLine(validateEmailExp.IsMatch(email));
        if (!validateEmailExp.IsMatch(email)) {
			return "Email is invalid";
		}

		if (phone is not null && phone.Length != 10)
		{
			return "Phone number must be 10 digits long";
		}

		return string.Empty;
	}
}
