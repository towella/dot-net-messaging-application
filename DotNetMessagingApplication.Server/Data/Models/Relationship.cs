using DotNetMessagingApplication.Server.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace DotNetMessagingApplication.Server.Data.Models;

[PrimaryKey(nameof(UserId), nameof(OtherPersonId))]
public class Relationship
{
	public int UserId { get; set; }

	public User? User { get; set; }

	public int OtherPersonId { get; set; }

	public User? OtherPerson { get; set; }

	public RelationshipType RelationshipType { get; set; } = RelationshipType.None;
}