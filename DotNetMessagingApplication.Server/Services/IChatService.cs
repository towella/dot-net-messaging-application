using DotNetMessagingApplication.Server.Data.Models;

public interface IChatService
{
    Task<Chat> CreateChat(int creatorId, IEnumerable<int> participantIds, string? chatName);
    Task<int> DeleteChat(int chatId);
    Task<IEnumerable<Chat>> GetChatsForUser(int userId);
}
