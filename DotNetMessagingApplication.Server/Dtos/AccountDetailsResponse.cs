using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace DotNetMessagingApplication.Server.Dtos;

[JsonObject]
public class AccountDetailsResponse
{
	[JsonProperty("username")]
	public string Username { get; set; } = string.Empty;

	[JsonProperty("email")]
	public string Email { get; set; } = string.Empty;

	[JsonProperty("phone")]
	public string? Phone { get; set; }

	[JsonProperty("pronouns")]
	public string? Pronouns { get; set; }

	[JsonProperty("profilePhotoLink")]
	public string? ProfilePhotoLink { get; set; }

	[JsonProperty("bio")]
	public string? Bio { get; set; }
}
