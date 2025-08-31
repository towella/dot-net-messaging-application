using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos;

[JsonObject]
public class LoginResponse
{
	[JsonProperty("success")]
	public bool Success { get; set; }

	[JsonProperty("message")]
	public string Message { get; set; } = null!;
}
