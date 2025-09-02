using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
	[JsonObject]
	public class DeleteChatRequest
	{
		[JsonProperty("chatId")]
		public int ChatId { get; set; }
		[JsonProperty("userId")]
		public int UserId { get; set; } // should only be able to delete as admin
	}
}
