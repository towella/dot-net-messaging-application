using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
    [JsonObject]
    public class DeleteMessageRequest
    {
        [JsonProperty("messageId")]
        public int MessageId { get; set; }
        [JsonProperty("userId")]
        public int UserID { get; set; } // makes sure user can only delete their own messages
    }
}
