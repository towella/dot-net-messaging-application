using DotNetMessagingApplication.Server.Dtos;
using DotNetMessagingApplication.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetMessagingApplication.Server.Controllers;

[ApiController]
[Route("api/controllers")]
public class LoginController(ILoginService loginService) : ControllerBase
{
	readonly ILoginService _loginService = loginService;

	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult Login([FromBody] LoginRequest request)
	{
		if (_loginService.Login(request.EmailOrUsername, request.Password))
		{
			return Ok("Login success!");
		}

		return NotFound("boooooooooooo");
	}
}
