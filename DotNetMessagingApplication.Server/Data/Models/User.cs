using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DotNetMessagingApplication.Server.Data.Models;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
	// basic info
	[Key]
	public int Id { get; set; }

	[Required]
	public string Password { get; set; } = null!;

	[Required]
	public string Username { get; set; } = null!;

	[Required]
	[EmailAddress]
	public string Email { get; set; } = null!;

	[StringLength(10)]
	public string? Phone { get; set; }

	public string? Pronouns { get; set; }

	public string? ProfilePhotoLink { get; set; }

	public string? Bio { get; set; }


	// social/relationships/misc config
	public ICollection<Relationship>? Following { get; set; }

	public ICollection<Relationship>? Followers { get; set; }

	public int SettingsId { get; set; }

	[Required]
	public Settings Settings { get; set; } = null!;

	public ICollection<Message>? MessagesSent { get; set; }

	public ICollection<Message>? MessagesReceived { get; set; }

	public ICollection<Reaction>? ReactionsGiven { get; set; }
}
