using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Dtos;
using DotNetMessagingApplication.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DotNetMessagingApplication.Server.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IMessageService _messageService;
		private readonly IChatService _chatService;

		public ChatHub(IMessageService messageService, IChatService chatService)
		{
			_messageService = messageService;
			_chatService = chatService;
		}

		#region messages
		public async Task SendMessage(SendMessageRequest sentMessage)
		{
			if (sentMessage == null)
			{
				await Clients.Caller.SendAsync("ReceiveError", "Message data is missing.");
				return;
			}

			var chat = await _chatService.GetChatById(sentMessage.ChatId);
			if (chat == null)
			{
				throw new Exception("Chat not found :(");
			}

			try
			{
				var message = new Message
				{
					MessageBody = sentMessage.Message,
					SenderId = sentMessage.SenderId,
					ChatId = sentMessage.ChatId,
					ImageUrl = sentMessage.ImageUrl,
					RecipientChat = chat,
					RecipientChatId = chat.ChatId,
					Chat = chat,
				};

				chat.Messages.Add(message);

				var savedMessage = _messageService.SendMessage(message);
				await _chatService.UpdateChat(chat);

				await Clients.Group(sentMessage.ChatId.ToString()).SendAsync("ReceiveMessage", savedMessage);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public async Task DeleteMessage(DeleteMessageRequest deletedMessage)
		{
			try
			{
				int messageId = deletedMessage.MessageId;
				int affectedRows = await _messageService.DeleteMessage(messageId);
				if (affectedRows > 0)
				{
					await Clients.All.SendAsync("MessageDeleted", messageId);
				}
				else
				{
					await Clients.Caller.SendAsync("ReceiveError", "Failed to delete message.");
				}
			}
			catch (Exception ex)
			{
				await Clients.Caller.SendAsync("ReceiveError", $"Error deleting message: {ex.Message}");
			}
		}

		public async Task EditMessage(EditMessageRequest editedMessage)
		{
			try
			{
				int messageId = editedMessage.MessageId;
				string newContent = editedMessage.NewMessage;

				int affectedRows = await _messageService.UpdateMessage(messageId, newContent);

				if (affectedRows == 1) // should only edit one row at a time
				{
					await Clients.All.SendAsync("MessageEdited", messageId, newContent);
				}
				else
				{
					await Clients.Caller.SendAsync("ReceiveError", "Failed to edit message.");
				}
			}
			catch (Exception ex)
			{
				await Clients.Caller.SendAsync("ReceiveError", $"Error editing message: {ex.Message}");
			}
		}

		#endregion

		#region chats
		public async Task CreateChat(CreateChatRequest request)
		{
			if (request == null)
			{
				await Clients.Caller.SendAsync("ReceiveError", "Chat data is missing.");
				return;
			}
			try
			{
				await _chatService.CreateChat(request.CreatorUser, request.ParticipantUsers, request.ChatName);
				await Clients.All.SendAsync("ChatCreated", request.ChatName);

			}
			catch (Exception ex)
			{
				await Clients.Caller.SendAsync("ReceiveError", $"Error creating group chat: {ex.Message}");
			}
		}

		public async Task<GetChatResponse> ChangeChat(GetChatRequest request)
		{
			var chat = await _chatService.GetChatById(request.ChatId);
			if (chat == null)
			{
                await Clients.Caller.SendAsync("ReceiveError", "Chat data is missing.");
				return new GetChatResponse();
            }
			var chatDto = new GetChatResponse
			{
				ChatId = chat.ChatId,
				ChatName = chat is GroupChat groupChat ? groupChat.ChatName : string.Empty,
				Messages = chat.Messages != null
					? chat.Messages.Select(m => new MessageDto
					{
						MessageId = m.MessageId,
						SenderUser = m.Sender.Username,
						Body = m.MessageBody
					}).ToList() : new List<MessageDto>(),
			};
			return chatDto;
        }

		public async Task DeleteChat(DeleteChatRequest request)
		{
			try
			{
				if (request == null)
				{
					await Clients.Caller.SendAsync("ReceiveError", "Chat data is missing.");
					return;
				}
				await _chatService.DeleteChat(request.ChatId);
				await Clients.All.SendAsync("ChatDeleted", request.ChatId);
			}
			catch (Exception ex)
			{
				await Clients.Caller.SendAsync("ReceiveError", $"Error deleting chat: {ex.Message}");
				return;
			}
		}

		public async Task GetChats(string username)
		{
			var chats = _chatService.GetChatsForUser(username);
			await Clients.Caller.SendAsync("ReceiveChats", chats);
		}

		public async Task<List<GetChatResponse>> GetDirectMessages(string username)
		{
			var chats = (IEnumerable<DirectMessage>) await _chatService.GetDirectMessagesForUser(username);

			var chatDtos = chats.Select(chat => new GetChatResponse
			{
				ChatId = chat.ChatId,
				ChatName = string.Empty,
				Messages = chat.Messages != null
					? chat.Messages.Select(m => new MessageDto
					{
						MessageId = m.MessageId,
						SenderUser = m.Sender.Username,
						Body = m.MessageBody

					}).ToList() : new List<MessageDto>(),
			}).ToList();

			return chatDtos;
		}

		public async Task<List<GetChatResponse>> GetGroupChats(string username)
		{
			var chats = (IEnumerable<GroupChat>) await _chatService.GetGroupChatsForUser(username);

			var chatDtos = chats.Select(chat => new GetChatResponse
			{
				ChatId = chat.ChatId,
				ChatName = chat.ChatName,
				Messages = chat.Messages != null
					? chat.Messages.Select(m => new MessageDto
				{
					MessageId = m.MessageId,
					SenderUser = m.Sender.Username,
					Body = m.MessageBody

				}).ToList() : new List<MessageDto>(),
			}). ToList();

			return chatDtos;
		}


		public async Task JoinChatGroup(int chatId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
		}

		public async Task LeaveChatGroup(int chatId)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
		}
		#endregion

		#region connections

		public override async Task OnConnectedAsync()
		{
			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			await base.OnDisconnectedAsync(exception);
		}
		#endregion
	}
}
