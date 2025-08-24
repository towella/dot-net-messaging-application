using DotNetMessagingApplication.Server.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetMessagingApplication.Server.Data.Models;

[PrimaryKey(nameof(SettingsId), nameof(UserId))]
public class Settings
{
	public int SettingsId { get; set; }

	[ForeignKey(nameof(UserId))]
	public int UserId { get; set; }

	public User User { get; set; } = null!;

	public TimeOnly TimeLimit { get; set; }

	public bool SendEmailNotifications { get; set; } = false;

	public PrivacyLevel PrivacyLevel { get; set; } = PrivacyLevel.Private;
}
