using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories.Base;

namespace DotNetMessagingApplication.Server.Data.Repositories;

public class UserRepository : Repository<User>
{
	public UserRepository(MessagingAppContext context) : base(context)
	{
	}

	public MessagingAppContext MessagingAppContext { get => _context as MessagingAppContext; }

	#region PUBLIC METHODS

	public User? GetUserByPasswordAndEmailOrUsername(string emailOrUsername, string password)
	{
		return MessagingAppContext.Users.SingleOrDefault(u => (u.Username == emailOrUsername || u.Email == emailOrUsername) && u.Password == password);
	}

	// only used after user logs in!
	public User? GetUserByEmailOrUsername(string emailOrUsername)
	{
		return MessagingAppContext.Users.SingleOrDefault(u => u.Username == emailOrUsername || u.Email == emailOrUsername);
	}

	public void AddUser(string username, string email, string password, string pronouns)
	{
		if (UsernameAlreadyExists(username) || EmailAlreadyExists(email))
		{
			throw new ArgumentException("Username or email already exists.");
		}

		Add(new User
		{
			Username = username,
			Email = email,
			Password = password,
			Pronouns = pronouns,
		});

		SaveChanges();
	}

	public void UpdateDetails(User updatedUser)
	{
		Update(updatedUser);
		SaveChanges();
	}

	public void SeedOneUserAndChild()
	{
		if (GetUserByEmailOrUsername("test") is null)
		{
			User user = new User
			{
				Username = "test",
				Password = "test",
				Email = "test@test.com"
			};

			Add(user);

			if (GetUserByEmailOrUsername("child") is null)
			{
				Add(new Child
				{
					Username = "child",
					Password = "child",
					Email = "child@child.com",
					Parent = user
				});
			}
			
			SaveChanges();
		}
	}

	#endregion


	#region HELPERS

	bool UsernameAlreadyExists(string username)
	{
		return MessagingAppContext.Users.Any(u => u.Username == username);
	}

	bool EmailAlreadyExists(string email)
	{
		return MessagingAppContext.Users.Any(u => u.Email == email);
	}

	#endregion
}
