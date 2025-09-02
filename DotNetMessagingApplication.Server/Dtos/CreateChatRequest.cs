using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
    [JsonObject]
    public class CreateChatRequest
    {
        [JsonProperty("chatName")]
        public string ChatName { get; set; } = string.Empty;
        [JsonProperty("participantIds")]
        public List<int> ParticipantIds { get; set; } = new List<int>();
        [JsonProperty("creatorId")]
        public int CreatorId { get; set; } // for group chats, the user creating the chat is the admin
    }
}
