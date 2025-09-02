using DotNetMessagingApplication.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetMessagingApplication.Server.Data;

public class MessagingAppContext : DbContext
{
	public virtual DbSet<Child> Children { get; set; }

	public virtual DbSet<DirectMessage> DirectMessages { get; set; }

	public virtual DbSet<GroupChat> GroupChats { get; set; }

	public virtual DbSet<Message> Messages { get; set; }

	public virtual DbSet<Reaction> Reactions { get; set; }

	public virtual DbSet<Relationship> Relationships { get; set; }

	public virtual DbSet<Settings> Settings { get; set; }

	public virtual DbSet<User> Users { get; set; }

	readonly string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "messaging.db");

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite($"Data Source={dbPath}");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// many to many self referencing relationship for Users
		modelBuilder.Entity<Relationship>()
			.HasOne(u => u.OtherPerson)
			.WithMany(u => u.Followers)
			.HasForeignKey(u => u.OtherPersonId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Relationship>()
			.HasOne(u => u.User)
			.WithMany(u => u.Following)
			.HasForeignKey(u => u.UserId);

		// one to one relationship for Users and Settings
		modelBuilder.Entity<User>()
			.HasOne(u => u.Settings)
			.WithOne(s => s.User)
			.HasForeignKey<Settings>(s => s.SettingsId);

		// Users to Messages mapping
		modelBuilder.Entity<Message>()
			.HasOne(m => m.Sender)
			.WithMany(u => u.MessagesSent)
			.HasForeignKey(m => m.SenderId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Message>()
			.HasOne(m => m.RecipientChat)
			.WithMany(u => u.Messages)
			.HasForeignKey(m => m.RecipientChatId);


		// direct messages
		modelBuilder.Entity<DirectMessage>()
			.HasOne(dm => dm.User)
			.WithMany(u => u.DirectMessageAsUser)
			.HasForeignKey(dm => dm.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<DirectMessage>()
			.HasOne(dm => dm.OtherPerson)
			.WithMany(u => u.DirectMessageAsOtherPerson)
			.HasForeignKey(dm => dm.OtherPersonId);


		// group chat admins (one admin per gc)
		modelBuilder.Entity<GroupChat>()
			.HasOne(gc => gc.Admin)
			.WithMany(a => a.GroupChatAdminOf)
			.HasForeignKey(gc => gc.AdminId)
			.OnDelete(DeleteBehavior.Restrict);

		// group chat members (many to many)
		modelBuilder.Entity<GroupChatMember>()
			.HasOne(m => m.GroupChat)
			.WithMany(gc => gc.Members)
			.HasForeignKey(m => m.GroupChatId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<GroupChatMember>()
			.HasOne(m => m.User)
			.WithMany(u => u.GroupChatMemberships)
			.HasForeignKey(m => m.UserId);
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder.Properties<string>().UseCollation("NOCASE");
	}
}
