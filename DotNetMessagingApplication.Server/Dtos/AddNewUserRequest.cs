using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos;

[JsonObject]
public class AddNewUserRequest
{
	[JsonProperty("username")]
	public string Username { get; set; } = null!;

	[JsonProperty("password")]
	public string Password { get; set; } = null!;

	[JsonProperty("email")]
	public string Email { get; set; } = null!;

	[JsonProperty("pronouns")]
	public string Pronouns { get; set; } = null!;
}
