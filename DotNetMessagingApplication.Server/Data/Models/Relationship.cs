using DotNetMessagingApplication.Server.Data.Enums;

namespace DotNetMessagingApplication.Server.Data.Models;

public class Relationship
{
	public int UserId { get; set; }

	public User? User { get; set; }

	public int OtherPersonId { get; set; }

	public User? OtherPerson { get; set; }

	public RelationshipType RelationshipType { get; set; } = RelationshipType.None;
}