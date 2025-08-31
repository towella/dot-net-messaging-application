using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DotNetMessagingApplication.Server.Data.Repositories
{
	public class ChatRepository : IChatRepository
	{
		protected readonly MessagingAppContext _context;
		public ChatRepository(MessagingAppContext context) 
		{
			_context = context;
		}

		public virtual async Task<Chat?> GetChatById(int chatId)
		{
			return await _context.Set<Chat>().FindAsync(chatId); // FindAsync() expects PKs to be passed in so no lambda expression needed. returns null if not found
		}

		public virtual async Task<int> DeleteChat(int chatId) // returns the number of chats deleted. ideally should be one
		{
			return await _context.Set<Chat>().Where(c => c.ChatId == chatId).ExecuteDeleteAsync();
		}

		public virtual async Task<int> AddChat(Chat chat)
		{
			await _context.Set<Chat>().AddAsync(chat);
			return await _context.SaveChangesAsync();
		}


	}
}
