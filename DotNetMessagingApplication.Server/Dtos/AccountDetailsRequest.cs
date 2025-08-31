using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos;

[JsonObject]
public class AccountDetailsRequest
{
	[JsonProperty("emailOrUsername")]
	public string EmailOrUsername { get; set; } = null!;
}
