using System.ComponentModel.DataAnnotations;

namespace DotNetMessagingApplication.Server.Data.Models;

public class Child : User
{
	[Required]
	public int ParentId { get; set; }

	public User Parent { get; set; } = null!;
}
