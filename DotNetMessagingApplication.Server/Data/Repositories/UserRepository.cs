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

	public void AddUser(string username, string email, string password)
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
		return MessagingAppContext.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
	}

	bool EmailAlreadyExists(string email)
	{
		return MessagingAppContext.Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
	}

	#endregion
}
