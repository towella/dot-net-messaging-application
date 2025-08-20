using DotNetMessagingApplication.Server.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace DotNetMessagingApplication.Server.Data.Models;

// User follows OtherPerson
// i.e. User is the follower, OtherPerson is being followed

[PrimaryKey(nameof(UserId), nameof(OtherPersonId))]
public class Relationship
{
	public int UserId { get; set; }

	public User? User { get; set; }

	public int OtherPersonId { get; set; }

	public User? OtherPerson { get; set; }

	public RelationshipType RelationshipType { get; set; } = RelationshipType.None;
}