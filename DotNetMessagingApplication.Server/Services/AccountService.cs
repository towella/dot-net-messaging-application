using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;
using System.Net.Mail;

namespace DotNetMessagingApplication.Server.Services;

public interface IAccountService
{
	void AddUser(string username, string email, string password, string pronouns);

	void UpdateDetails(User user);

	User GetDetails(string emailOrPassword);
}

public class AccountService(IUserRepository userRepo) : IAccountService
{
	readonly IUserRepository _userRepo = userRepo;

	public void AddUser(string username, string email, string password, string pronouns)
	{
		if (!MailAddress.TryCreate(email, out var _))
		{
			throw new ArgumentException("Email was invalid.");
		}

		_userRepo.AddUser(username, email, password, pronouns);
	}

	public void UpdateDetails(User user)
	{
		_userRepo.UpdateDetails(user);
	}

	public User GetDetails(string emailOrPassword)
	{
		// seeding, temporary for testing
		_userRepo.SeedOneUserAndChild();


		User? user = _userRepo.GetUserByEmailOrUsername(emailOrPassword);

		if (user is null)
		{
			throw new InvalidOperationException("User does not exist.");
		}

		return user;
	}
}
