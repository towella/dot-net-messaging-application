using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;

namespace DotNetMessagingApplication.Server.Services
{
	public class MessageService : IMessageService
	{
		private readonly MessageRepository _messageRepository;

		public MessageService(MessageRepository messageRepository)
		{
			_messageRepository = messageRepository;
		}

		public async Task<int> SendMessage(Message message)
		{
			return await _messageRepository.AddMessage(message);
		}

		public async Task<int> UpdateMessage(int messageId, string newContent)
		{
			try
			{
				var message = await _messageRepository.GetMessageById(messageId);
				if (message == null)
				{
					throw new Exception("Message not found :(");
                }
                int senderId = message.SenderId;
				await _messageRepository.EditMessage(messageId, senderId, newContent);
				return 1;
			}
			catch
			{
				return 0;
			}
		}

		public async Task<int> DeleteMessage(int messageId)
		{
			try
			{
				var message = _messageRepository.GetMessageById(messageId);
				if (message == null)
				{
					throw new Exception("Message not found :(");
				}
				return await _messageRepository.DeleteMessage(messageId);
			}
			catch
			{
				return 0;
            }
        }
	}
}
