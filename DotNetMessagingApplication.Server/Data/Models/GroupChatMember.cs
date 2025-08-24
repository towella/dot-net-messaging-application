using DotNetMessagingApplication.Server.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DotNetMessagingApplication.Server.Data.Models;

// associative entity for group chats and users
[PrimaryKey(nameof(GroupChatId), nameof(UserId))]
public class GroupChatMember
{
	public int GroupChatId { get; set; }

	[Required]
	public GroupChat GroupChat { get; set; } = null!;

	public int UserId { get; set; }

	[Required]
	public User User { get; set; } = null!;

	public GroupChatRole Role { get; set; } = GroupChatRole.Member;
}
