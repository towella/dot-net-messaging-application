namespace DotNetMessagingApplication.Server.Dtos
{
    public class DeleteChatDto
    {
        public int ChatId { get; set; }
        public int UserId { get; set; } // should only be able to delete as admin?
    }
}
