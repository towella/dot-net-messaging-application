
using Newtonsoft.Json;

namespace DotNetMessagingApplication.Server.Dtos
{
    [JsonObject]
    public class MessageDto
    {
        [JsonProperty("messageId")]
        public int MessageId { get; set; }
        [JsonProperty("senderUser")]
        public string SenderUser { get; set; } = null!;
        [JsonProperty("body")]
        public string Body { get; set; } = null!;
    }

    [JsonObject]
    public class GetChatResponse
    {
        [JsonProperty("chatId")]
        public int ChatId { get; set; }
        [JsonProperty("chatName")]
        public string? ChatName { get; set; }
        [JsonProperty("messages")]
        public List<MessageDto> Messages { get; set;} = new List<MessageDto>();
    }
}
