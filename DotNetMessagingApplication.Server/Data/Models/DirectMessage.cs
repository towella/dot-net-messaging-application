namespace DotNetMessagingApplication.Server.Data.Models;

public class DirectMessage : Chat
{
	public int UserId { get; set; }

	public User User { get; set; } = null!;

	public int OtherPersonId { get; set; }

	public User OtherPerson { get; set; } = null!;
}
