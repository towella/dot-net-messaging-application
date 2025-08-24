namespace DotNetMessagingApplication.Server.Services
{
	public class MessageService : IMessageService
	{
		public async Task<int> CreateMessage()
		{
			// first get the conversation the logged in user wants to end a message

			// then get the message body from FE, create a new message
			return 0;
		}
		public async Task<int> UpdateMessage()
		{
            // first get the conversation the logged in user wants to end a message

			// then get the message from messageID

			// change the message, update and save changes to DB

			return 0;
        }
        public async Task DeleteMessage()
		{
            // first get the conversation the logged in user wants to end a message

            // then get the message from messageID

			// then delete the message

			// do we want a placeholder like 'this message has been deleted'  ??
        }
        public async Task SendMessage()
		{
			// idk if this should be here tbh
		}

		public async Task<int> GetUnreadMessages(int userId) // could be used to display in the html title? e.g. blahblah (9+ unread)
		{
			int unreadMessageCount = 0;
			// load all the chats containing the logged in user

			// idk how yet but loop through the chats to find unread messages, add them up

			return unreadMessageCount;
		}
	}
}
