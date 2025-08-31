namespace DotNetMessagingApplication.Server.Services
{
    public interface IMessageService
    {
        // these might be subject to change...
        Task<int> CreateMessage();
        Task<int> UpdateMessage(int messageId);

        Task DeleteMessage(int messageId);

        Task SendMessage();
        Task<int> GetUnreadMessages(int userId);
    }
}
