namespace DotNetMessagingApplication.Server.Dtos
{
    public class DeleteMessageDto
    {
        public int MessageId { get; set; }
        public int UserID { get; set; } // makes sure user can only delete their own messages
    }
}
