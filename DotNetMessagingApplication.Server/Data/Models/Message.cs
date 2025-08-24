using System.ComponentModel.DataAnnotations;

namespace DotNetMessagingApplication.Server.Data.Models;

public class Message
{
	[Key]
	public int MessageId { get; set; }

	[Required]
	public int SenderId { get; set; }
	public User Sender { get; set; } = null!;

	[Required]
	public int RecipientId { get; set; }

	public User Recipient { get; set; } = null!;

	[Required]
	public string MessageBody { get; set; } = null!;

	public DateTime TimeSent { get; set; }

	public int ChatId { get; set; }

	[Required]
	public Chat Chat { get; set; } = null!;
}