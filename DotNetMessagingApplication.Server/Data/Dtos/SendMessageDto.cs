namespace DotNetMessagingApplication.Server.Data.Dtos
{
    public class SendMessageDto
    {
        public string Message { get; set; }
        public int SenderId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int ChatId { get; set; }
    }
}
