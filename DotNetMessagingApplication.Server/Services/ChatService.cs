using DotNetMessagingApplication.Server.Data;
using DotNetMessagingApplication.Server.Data.Enums;
using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;
using DotNetMessagingApplication.Server.Data.Repositories.Base;
using DotNetMessagingApplication.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace DotNetMessagingApplication.Server.Services
{
	public class ChatService : IChatService
	{
		private readonly ChatRepository _chatRepository;
		private readonly MessagingAppContext _context;
		private readonly IUserRepository _userRepository;

		public ChatService(ChatRepository chatRepository, MessagingAppContext context, IUserRepository userRepository)
		{
			_chatRepository = chatRepository;
			_context = context;
			_userRepository = userRepository;
		}

		public async Task<Chat> CreateChat(string creatorUser, IEnumerable<string> participantUsers, string? chatName)
		{
			var creator = _userRepository.GetUserByEmailOrUsername(creatorUser);
			int count = participantUsers.Count();

			if (count < 1) // since the enum doesn't contain the creator
			{ 
				throw new ArgumentException("A chat must have at least two participants."); 
			}
			else if (count == 1)
			{
				var otherUser = _userRepository.GetUserByEmailOrUsername(participantUsers.ElementAt(0));
				if (otherUser is null)
					throw new ArgumentException("other user not found.");
				try
				{
					return await CreateDirectMessage(creator!, otherUser);
				}
				catch (Exception ex)
				{
					throw new Exception("Error creating direct message: " + ex.Message);
				}
			}
			else
			{
				if (chatName is null)
				{ 
					throw new ArgumentException("A group chat must have a name."); 
				}
				try
				{
					return await CreateGroupChatAsync(creator!, participantUsers, chatName);
				}
				catch (Exception ex)
				{
					throw new Exception("Error creating group chat: " + ex.Message);
				}
			}
		}

		public async Task<Chat> CreateDirectMessage(User user1, User user2)
		{
			var directMessage = new DirectMessage
			{
				UserId = user1.Id,
				OtherPersonId = user2.Id,
				User = user1,
				OtherPerson = user2,
				Messages = new List<Message>()
            };

			return await _chatRepository.CreateDirectMessage(directMessage);
		}

		public async Task<Chat> CreateGroupChatAsync(User admin, IEnumerable<string> chatParticipants, string chatName)
		{
			if (chatParticipants.Count() < 2)
				throw new ArgumentException("A group chat must have at least three participants.");

			var groupChat = new GroupChat
			{
				AdminId = admin.Id,
				Admin = admin,
				ChatName = chatName,
				Messages = new List<Message>()
			};

			var members = new List<GroupChatMember>
			{
				new GroupChatMember
				{
					GroupChatId = groupChat.ChatId,
					UserId = admin.Id,
					User = admin,
					Role = GroupChatRole.Admin
				}
			};

			foreach (var username in chatParticipants)
			{
				var user = _userRepository.GetUserByEmailOrUsername(username);
				if (user == null)
				{
					throw new ArgumentException($"User '{username}' not found.");
				}

				members.Add(new GroupChatMember
				{
					GroupChatId = groupChat.ChatId,
					UserId = user.Id,
					User = user,
					Role = GroupChatRole.Member
				});
			}

			groupChat.Members = members;
			return await _chatRepository.CreateGroupChat(groupChat, members);
		}

		public async Task<int> DeleteChat(int chatId)
		{
			return await _chatRepository.DeleteChat(chatId);
		}

		public async Task<Chat?> GetChatById(int chatId)
		{
			return await _chatRepository.GetChatById(chatId);
        }

        public async Task<IEnumerable<Chat>> GetChatsForUser(string username)
		{
			return await _chatRepository.GetChatsForUser(username);
		}

		public async Task<IEnumerable<Chat>> GetDirectMessagesForUser(string username)
		{
			return await _chatRepository.GetDirectMessagesForUser(username);
        }
		public async Task<IEnumerable<Chat>> GetGroupChatsForUser(string username)
		{
			return await _chatRepository.GetGroupChatsForUser(username);
		}

		public async Task<int> UpdateChat(Chat chat)
		{
			return await _chatRepository.UpdateChat(chat);
        }
	}
}
