using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
    [JsonObject]
    public class GetChatRequest
    {
        [JsonProperty("chatId")]
        public int ChatId { get; set; }
    }
}
