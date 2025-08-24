namespace DotNetMessagingApplication.Server.Services
{
    public interface IMessageService
    {
        // these might be subject to change...
        Task<int> CreateMessage();
        Task<int> UpdateMessage();

        Task DeleteMessage();

        Task SendMessage();
        Task<int> GetUnreadMessages(int userId);
    }
}
