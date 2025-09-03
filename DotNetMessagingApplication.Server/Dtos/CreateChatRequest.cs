using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
	[JsonObject]
	public class CreateChatRequest
	{
		[JsonProperty("chatName")]
		public string ChatName { get; set; } = null!;
		[JsonProperty("participantIds")]
		public List<string> ParticipantUsers { get; set; } = new List<string>();

		[JsonProperty("creatorUser")]
		public string CreatorUser { get; set; } = null!; // for group chats, the user creating the chat is the admin
	}
}
