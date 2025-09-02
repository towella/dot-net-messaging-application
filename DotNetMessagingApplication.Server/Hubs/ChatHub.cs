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
		public async Task SendMessage([FromBody] SendMessageRequest sentMessage)
		{
			if (sentMessage == null)
			{
				await Clients.Caller.SendAsync("ReceiveError", "Message data is missing.");
				return;
			}

			var message = new Message
			{
				MessageBody = sentMessage.Message,
				SenderId = sentMessage.SenderId,
				ChatId = sentMessage.ChatId
			};

			var savedMessage = _messageService.SendMessage(message);

			await Clients.Group(sentMessage.ChatId.ToString())
						 .SendAsync("ReceiveMessage", savedMessage);
		}

		public async Task DeleteMessage([FromBody] DeleteMessageRequest deletedMessage)
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

		public async Task EditMessage([FromBody] EditMessageRequest editedMessage)
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
		public async Task CreateGroupChat([FromBody] CreateChatRequest request)
		{
			if (request == null)
			{
				await Clients.Caller.SendAsync("ReceiveError", "Chat data is missing.");
				return;
			}
			try
			{
				await _chatService.CreateChat(request.CreatorId, request.ParticipantIds, request.ChatName);
                await Clients.All.SendAsync("ChatCreated", request.ChatName);

            }
            catch (Exception ex)
			{
				await Clients.Caller.SendAsync("ReceiveError", $"Error creating group chat: {ex.Message}");
            }
        }

		public async Task DeleteChat([FromBody] DeleteChatRequest request)
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

		public async Task GetChats(int userId)
		{
			var chats = _chatService.GetChatsForUser(userId);
			await Clients.Caller.SendAsync("ReceiveChats", chats);
		}

		public async Task GetDirectMessages(int userId)
		{
			var chats = _chatService.GetDirectMessagesForUser(userId);
			await Clients.Caller.SendAsync("ReceiveDms", chats);
        }

		public async Task GetGroupChats(int userId)
		{
			var chats = _chatService.GetGroupChatsForUser(userId);
			await Clients.Caller.SendAsync("ReceiveGroupChats", chats);
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
