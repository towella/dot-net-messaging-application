using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos;

[JsonObject]
public class LoginRequest
{
	[JsonProperty("emailOrUsername")]
	public string EmailOrUsername { get; } = null!;

	[JsonProperty("password")]
	public string Password { get; } = null!;
}
