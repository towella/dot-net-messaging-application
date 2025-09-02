using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos;

[JsonObject]
public class UpdateUserRequest
{
	[JsonProperty("oldUsername")]
	public string OldUsername { get; set; } = null!;

	[JsonProperty("username")]
	public string Username { get; set; } = null!;

	[JsonProperty("email")]
	public string Email { get; set; } = null!;

	[JsonProperty("phone")]
	public string Phone {  get; set; } = null!;

	[JsonProperty("pronouns")]
	public string Pronouns { get; set; } = null!;

	[JsonProperty("bio")]
	public string Bio {  get; set; } = null!;
}
