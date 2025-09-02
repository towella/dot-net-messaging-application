using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
    [JsonObject]
    public class RemoveChatParticipantRequest
    {
        [JsonProperty("chatId")]
        public int ChatId { get; set; }
        [JsonProperty("userIdToRemove")]
        public int UserIdToRemove { get; set; }
        [JsonProperty("adminUserId")]
        public int AdminUserId { get; set; } // the user making the request, should be an admin for that chat
    }
}
