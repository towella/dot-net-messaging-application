using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos;

[JsonObject]
public class LoginRequest
{
	[JsonProperty("emailOrUsername")]
	public string EmailOrUsername { get; set; } = null!;

	[JsonProperty("password")]
	public string Password { get; set; } = null!;
}
