using System.ComponentModel.DataAnnotations;

namespace DotNetMessagingApplication.Server.Data.Models;

public class GroupChat : Chat
{
	[Required]
	public string ChatName { get; set; } = null!;

	public int AdminId { get; set; }

	[Required]
	public User Admin { get; set; } = null!;

	public ICollection<GroupChatMember> Members { get; set; } = null!;
}
