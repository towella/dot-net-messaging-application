namespace DotNetMessagingApplication.Server.Data.Models;

public abstract class Chat
{
	public int ChatId { get; set; }

	public int UserId { get; set; }

	public User User { get; set; } = null!;

	public ICollection<Message>? Messages { get; set; }
}
