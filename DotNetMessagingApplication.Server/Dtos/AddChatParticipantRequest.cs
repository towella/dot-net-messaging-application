using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
    [JsonObject]
    public class AddChatParticipantRequest
    {
        [JsonProperty("chatId")]
        public int ChatId { get; set; }
        [JsonProperty("userIdToAdd")]
        public int UserIdToAdd { get; set; }
        [JsonProperty("adminUserId")]
        public int AdminUserId { get; set; } // the user making the request, should be an admin for that chat
    }
}
