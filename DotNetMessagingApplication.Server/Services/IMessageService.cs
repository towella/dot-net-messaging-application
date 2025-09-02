using DotNetMessagingApplication.Server.Data.Models;

namespace DotNetMessagingApplication.Server.Services
{
	public interface IMessageService
	{
		// these might be subject to change...
		Task<int> UpdateMessage(int messageId, string content);
		Task<int> DeleteMessage(int messageId);
		Task<int> SendMessage(Message message);
		Task<int> SendMessage(Message messgae, IFormFile imageFile);
	}
}
