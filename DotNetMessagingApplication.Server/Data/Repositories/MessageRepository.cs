using DotNetMessagingApplication.Server.Data.Models;

namespace DotNetMessagingApplication.Server.Data.Repositories
{
	public class MessageRepository
	{
		private readonly MessagingAppContext _messagingAppContext;

		public MessageRepository(MessagingAppContext messagingAppContext)
		{
			_messagingAppContext = messagingAppContext;
		}
	    private int SaveMessagesToDatabase(IEnumerable<Message> messages) // return number of entries written to db
		{
			_messagingAppContext.Messages.AddRange(messages);
			return _messagingAppContext.SaveChanges();
		}

		public void AddMessage(Message message)
		{
			_messagingAppContext.Messages.Add(message);
			_messagingAppContext.SaveChanges();
		}

		public void DeleteMessage(Message message)
		{
			_messagingAppContext.Remove(message);
			_messagingAppContext.SaveChanges();
		}

		public void EditMessage(int messageId, int userId, string newBody)
		{
			Message? message = _messagingAppContext.Messages.FirstOrDefault(m => m.MessageId == messageId && m.SenderId == userId);
			if (message != null)
			{
				message.MessageBody = newBody;
				_messagingAppContext.Update(message);
				_messagingAppContext.SaveChanges();
			}
			else
			{
				throw new Exception("the message u wanted to edit wasn't found D:");
			}
		}
	}
}
