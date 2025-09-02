using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
	[JsonObject]
	public class SendMessageRequest
	{
		[JsonProperty("message")]
		public string Message { get; set; }
		[JsonProperty("senderId")]
		public int SenderId { get; set; }
		[JsonProperty("chatId")]
		public int ChatId { get; set; }
		[JsonProperty("imageUrl")]
		public string? ImageUrl { get; set; }
    }
}
