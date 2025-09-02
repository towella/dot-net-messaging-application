using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
	[JsonObject]
	public class EditMessageRequest
	{
		[JsonProperty("messageId")]
		public int MessageId { get; set; }
		[JsonProperty("newMessage")]
		public string NewMessage { get; set; }
		[JsonProperty("userId")]
		public int UserId { get; set; } // makes sure user can only edit their own messages
	}
}
