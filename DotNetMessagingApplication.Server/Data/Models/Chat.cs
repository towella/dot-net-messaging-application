namespace DotNetMessagingApplication.Server.Data.Models;

public abstract class Chat
{
	public int ChatId { get; set; }

	public ICollection<Message> Messages { get; set; } = new List<Message>();
}
