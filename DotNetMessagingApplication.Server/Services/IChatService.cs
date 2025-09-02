using DotNetMessagingApplication.Server.Data.Models;

public interface IChatService
{
	Task<Chat> CreateChat(string creatorUser, IEnumerable<string> participantUsers, string? chatName);
	Task<int> DeleteChat(int chatId);
	Task<IEnumerable<Chat>> GetChatsForUser(int userId);
	Task<IEnumerable<Chat>> GetDirectMessagesForUser(int userId);
	Task<IEnumerable<Chat>> GetGroupChatsForUser(int userId);
}