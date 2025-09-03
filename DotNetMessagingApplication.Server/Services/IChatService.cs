using DotNetMessagingApplication.Server.Data.Models;

public interface IChatService
{
	Task<Chat> CreateChat(string creatorUser, IEnumerable<string> participantUsers, string? chatName);
	Task<int> DeleteChat(int chatId);
	Task<IEnumerable<Chat>> GetChatsForUser(string username);
	Task<IEnumerable<Chat>> GetDirectMessagesForUser(string username);
	Task<IEnumerable<Chat>> GetGroupChatsForUser(string username);
	Task<Chat?> GetChatById(int chatId);
	Task<int> UpdateChat(Chat chat);
}