namespace DotNetMessagingApplication.Server.Data.Models;

public class DirectMessage : Chat
{
	public int OtherPersonId { get; set; }

	public User OtherPerson { get; set; } = null!;
}
