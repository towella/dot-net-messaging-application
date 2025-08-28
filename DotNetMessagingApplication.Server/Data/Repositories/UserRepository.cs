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
		if (updatedUser == null) {
			throw new ArgumentException("user object was null");
		}

		if (updatedUser.Username == null || updatedUser.Email == null || updatedUser.Password == null) {
			throw new ArgumentException("required fields were null.");
		}

		Update(updatedUser);
		SaveChanges();
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
