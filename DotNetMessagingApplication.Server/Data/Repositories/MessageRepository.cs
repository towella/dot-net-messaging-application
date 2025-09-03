using DotNetMessagingApplication.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetMessagingApplication.Server.Data.Repositories
{
	public class MessageRepository
	{
		private readonly MessagingAppContext _messagingAppContext;

		public MessageRepository(MessagingAppContext messagingAppContext)
		{
			_messagingAppContext = messagingAppContext;
		}

		private async Task<int> SaveMessagesToDatabase(IEnumerable<Message> messages)
		{
			await _messagingAppContext.Messages.AddRangeAsync(messages);
			return await _messagingAppContext.SaveChangesAsync();
		}

		public async Task<int> AddMessage(Message message)
		{
            _messagingAppContext.Set<Message>().Add(message);
            return await _messagingAppContext.SaveChangesAsync(); // Save to generate ChatId
        }

		public async Task<int> DeleteMessage(int messageId)
		{
			Message? message = _messagingAppContext.Messages.Where(m => m.MessageId == messageId).FirstOrDefault();
			if (message == null)
			{
				throw new Exception("The message you wanted to delete wasn't found D:");
			}
			return await _messagingAppContext.SaveChangesAsync();
		}

		public async Task<int> EditMessage(int messageId, int userId, string newBody)
		{
			var message = await _messagingAppContext.Messages
				.FirstOrDefaultAsync(m => m.MessageId == messageId && m.SenderId == userId);

			if (message != null)
			{
				message.MessageBody = newBody;
				_messagingAppContext.Messages.Update(message);
				return await _messagingAppContext.SaveChangesAsync();
			}
			else
			{
				throw new Exception("The message you wanted to edit wasn't found D:");
			}
		}

		public async Task<Message?> GetMessageById(int messageId)
		{
			return await _messagingAppContext.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
		}

	}
}
	   