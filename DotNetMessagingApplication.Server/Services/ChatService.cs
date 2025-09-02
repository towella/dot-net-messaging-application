using DotNetMessagingApplication.Server.Data;
using DotNetMessagingApplication.Server.Data.Enums;
using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;
using DotNetMessagingApplication.Server.Data.Repositories.Base;
using DotNetMessagingApplication.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace DotNetMessagingApplication.Server.Services
{
	public class ChatService : IChatService
	{
		private readonly IChatRepository _chatRepository;
		private readonly MessagingAppContext _context;

		public ChatService(IChatRepository chatRepository, MessagingAppContext context)
		{
			_chatRepository = chatRepository;
			_context = context;
		}

		public async Task<Chat> CreateChat(int creatorId, IEnumerable<int> participantIds, string chatName)
		{
			int count = participantIds.Count();

			if (count < 2)
				throw new ArgumentException("A chat must have at least two participants.");

			if (count == 2)
			{
				var user1 = participantIds.ElementAt(0);
				var user2 = participantIds.ElementAt(1);
				return await CreateDirectMessage(user1, user2);
			}

			return await CreateGroupChatAsync(creatorId, participantIds, chatName);
		}


		public async Task<Chat> CreateDirectMessage(int user1Id, int user2Id)
		{
			var user1 = await _context.Set<User>().FindAsync(user1Id);
			var user2 = await _context.Set<User>().FindAsync(user2Id);

			if (user1 == null || user2 == null)
				throw new ArgumentException("One or both users not found.");

			var directMessage = new DirectMessage
			{
				UserId = user1Id,
				OtherPersonId = user2Id,
				User = user1,
				OtherPerson = user2
			};

			return await _chatRepository.CreateDirectMessage(directMessage);
		}

		public async Task<Chat> CreateGroupChatAsync(int adminId, IEnumerable<int> chatParticipants, string chatName)
		{
			if (chatParticipants.Count() < 2)
				throw new ArgumentException("A group chat must have at least two participants.");

			var adminUser = await _context.Set<User>().FindAsync(adminId);
			if (adminUser == null)
				throw new ArgumentException("Admin user not found.");

			var groupChat = new GroupChat
			{
				AdminId = adminId,
				Admin = adminUser,
				ChatName = chatName
			};

			await _context.Set<GroupChat>().AddAsync(groupChat);
			await _context.SaveChangesAsync(); // generate ChatId

			var members = new List<GroupChatMember>
			{
				new GroupChatMember
				{
					GroupChatId = groupChat.ChatId,
					UserId = adminId,
					User = adminUser,
					Role = GroupChatRole.Admin
				}
			};

			foreach (var participantId in chatParticipants)
			{
				if (participantId == adminId) continue;

				var user = await _context.Set<User>().FindAsync(participantId);
				if (user == null) continue;

				members.Add(new GroupChatMember
				{
					GroupChatId = groupChat.ChatId,
					UserId = participantId,
					User = user,
					Role = GroupChatRole.Member
				});
			}

			return await _chatRepository.CreateGroupChat(groupChat, members);
		}

		public async Task<int> UpdateChatAsync(int chatId)
		{
			// Placeholder for future update logic
			return 0;
		}

		public async Task<int> DeleteChat(int chatId)
		{
			return await _chatRepository.DeleteChat(chatId);
		}

		public async Task<IEnumerable<Chat>> GetChatsForUser(int userId)
		{
			return await _chatRepository.GetChatsForUser(userId);
		}

		public async Task<IEnumerable<Chat>> GetDirectMessagesForUser(int userId)
		{
			var chats = await _chatRepository.GetChatsForUser(userId);
			return chats.Where(c => c is DirectMessage);
		}
		public async Task<IEnumerable<Chat>> GetGroupChatsForUser(int userId)
		{
			var chats = await _chatRepository.GetChatsForUser(userId);
			return chats.Where(c => c is GroupChat);
		}
	}
}
