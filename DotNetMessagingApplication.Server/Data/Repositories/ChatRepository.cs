using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DotNetMessagingApplication.Server.Data.Repositories
{
	public class ChatRepository
	{
		private readonly MessagingAppContext _context;

		public ChatRepository(MessagingAppContext context)
		{
			_context = context;
		}
		public async Task<Chat?> GetChatById(int chatId)
		{
			var chat = await _context.Set<Chat>()
				.Include(c => (c as DirectMessage)!.User)
				.Include(c => (c as DirectMessage)!.OtherPerson)
				.Include(c => (c as GroupChat)!.Admin)
				.Include(c => (c as GroupChat)!.Members)
				.ThenInclude(m => m.User)
				.Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.ChatId == chatId);
			return chat;
		}

		public async Task<DirectMessage> CreateDirectMessage(DirectMessage directMessage)
		{
			_context.Set<DirectMessage>().Add(directMessage);
			await _context.SaveChangesAsync();
			return directMessage;
		}

		public async Task<GroupChat> CreateGroupChat(GroupChat groupChat, IEnumerable<GroupChatMember> members)
		{
			_context.Set<GroupChat>().Add(groupChat);
			await _context.SaveChangesAsync(); // Save to generate ChatId

			foreach (var member in members)
			{
				member.GroupChatId = groupChat.ChatId;
            }

            _context.Set<GroupChatMember>().AddRange(members);
			await _context.SaveChangesAsync();

			return groupChat;
		}

		public async Task<int> DeleteChat(int chatId)
		{
			return await _context.Set<Chat>()
				.Where(c => c.ChatId == chatId)
				.ExecuteDeleteAsync();
		}

		public async Task<IEnumerable<Chat>> GetChatsForUser(string username)
		{
			var directMessages = await _context.Set<DirectMessage>()
				.Include(dm => dm.User)
				.Include(dm => dm.OtherPerson)
				.Where(dm => dm.User.Username == username || dm.OtherPerson.Username == username)
				.ToListAsync();

			var groupChats = await _context.Set<GroupChat>()
				.Include(gc => gc.Members)
				.ThenInclude(m => m.User)
				.Where(gc => gc.Members.Any(m => m.User.Username == username))
				.ToListAsync();

			return directMessages.Cast<Chat>().Concat(groupChats.Cast<Chat>());
		}

		public async Task<IEnumerable<Chat>> GetDirectMessagesForUser(string username)
		{
            var directMessages = await _context.Set<DirectMessage>()
                .Include(dm => dm.User)
                .Include(dm => dm.OtherPerson)
                .Where(dm => dm.User.Username == username || dm.OtherPerson.Username == username)
                .ToListAsync();
			return directMessages.Cast<Chat>();
        }
		public async Task<IEnumerable<Chat>> GetGroupChatsForUser(string username)
		{
            var groupChats = await _context.Set<GroupChat>()
                .Include(gc => gc.Members)
                .ThenInclude(m => m.User)
                .Where(gc => gc.Members.Any(m => m.User.Username == username))
                .ToListAsync();
			return groupChats.Cast<Chat>();
        }

		public async Task<int> UpdateChat(Chat chat)
		{
			await _context.Set<Chat>().Where(c => c.ChatId == chat.ChatId)
				.ExecuteUpdateAsync(c => c
					.SetProperty(c => c.Messages, chat.Messages)
                );
            return await _context.SaveChangesAsync();
        }
    }
}
