using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Dtos;
using DotNetMessagingApplication.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Controllers;

[ApiController]
[Route("api/controllers")]
public class AccountController(IAccountService accountService) : ControllerBase
{
	readonly IAccountService _accountService = accountService;

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
