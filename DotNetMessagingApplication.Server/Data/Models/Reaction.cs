using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetMessagingApplication.Server.Data.Models;

[PrimaryKey(nameof(MessageId), nameof(UserId))]
public class Reaction
{
	[ForeignKey(nameof(MessageId))]
	public int MessageId { get; set; }

	public Message Message { get; set; } = null!;

	[ForeignKey(nameof(UserId))]
	public int UserId { get; set; }

	public User User { get; set; } = null!;

	[Required]
	public string Icon { get; set; } = null!;
}
