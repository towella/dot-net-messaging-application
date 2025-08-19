using DotNetMessagingApplication.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetMessagingApplication.Server.Data;

public class MessagingAppContext : DbContext
{
	public DbSet<User> Users { get; set; }

	readonly string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "messaging.db");

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite($"Data Source={dbPath}");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// composite FK for Relationship entity
		modelBuilder.Entity<Relationship>()
			.HasKey(f => new { f.UserId, f.OtherPersonId });

		// many to many self referencing relationship for Users
		modelBuilder.Entity<Relationship>()
			.HasOne(f => f.OtherPerson)
			.WithMany(f => f.Following)
			.HasForeignKey(f => f.OtherPersonId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Relationship>()
			.HasOne(f => f.User)
			.WithMany(f => f.Followers)
			.HasForeignKey(f => f.UserId);

		// one to one relationship for Users and Settings
		modelBuilder.Entity<User>()
			.HasOne(e => e.Settings)
			.WithOne(e => e.User)
			.HasForeignKey<Settings>(s => s.SettingsId);
	}
}
