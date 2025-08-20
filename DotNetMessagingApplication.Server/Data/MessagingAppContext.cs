using DotNetMessagingApplication.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetMessagingApplication.Server.Data;

public class MessagingAppContext : DbContext
{
	public DbSet<Child> Children { get; set; }

	public DbSet<Message> Messages { get; set; }

	public DbSet<Relationship> Relationships { get; set; }

	public DbSet<Settings> Settings { get; set; }

	public DbSet<User> Users { get; set; }

	readonly string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "messaging.db");

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite($"Data Source={dbPath}");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// many to many self referencing relationship for Users
		modelBuilder.Entity<Relationship>()
			.HasOne(f => f.OtherPerson)
			.WithMany(f => f.Followers)
			.HasForeignKey(f => f.OtherPersonId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Relationship>()
			// User = left hand col of associative entity
			.HasOne(f => f.User)
			// User.Following: list where User is the left hand col
			// therefore one User can have multiple diff users in the right hand col
			.WithMany(f => f.Following)
			.HasForeignKey(f => f.UserId);

		// one to one relationship for Users and Settings
		modelBuilder.Entity<User>()
			.HasOne(e => e.Settings)
			.WithOne(e => e.User)
			.HasForeignKey<Settings>(s => s.SettingsId);

		// Users to Messages mapping
		modelBuilder.Entity<Message>()
			.HasOne(e => e.Sender)
			.WithMany(e => e.MessagesSent)
			.HasForeignKey(e => e.SenderId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Message>()
			.HasOne(e => e.Recipient)
			.WithMany(e => e.MessagesReceived)
			.HasForeignKey(e => e.RecipientId);
	}
}
