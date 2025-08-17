using DotNetMessagingApplication.Server.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetMessagingApplication.Server.Data.Models;

[Index(nameof(Username), IsUnique = true)]
public class User
{
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

	public ICollection<Relationship>? Following { get; set; }

	public ICollection<Relationship>? Followers { get; set; }

	public int SettingsId { get; set; }

	public Settings Settings { get; set; } = null!;
}

public class Relationship
{
	public int UserId { get; set; }

	public User? User { get; set; }

	public int OtherPersonId { get; set; }

	public User? OtherPerson { get; set; }

	public RelationshipType RelationshipType { get; set; } = RelationshipType.None;
}
