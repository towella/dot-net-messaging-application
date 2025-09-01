using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;
using System.Net.Mail;

namespace DotNetMessagingApplication.Server.Services;

public interface IAccountService
{
	void AddUser(string username, string email, string password, string pronouns);

	void UpdateDetails(User newDetails);

	User GetDetails(string emailOrPassword);
}

public class AccountService(IUserRepository userRepo) : IAccountService
{
	readonly IUserRepository _userRepo = userRepo;

	public void AddUser(string username, string email, string password, string pronouns)
	{
		if (!MailAddress.TryCreate(email, out _))
		{
			throw new ArgumentException("Email was invalid.");
		}

		_userRepo.AddUser(username, email, password, pronouns);
	}

	public void UpdateDetails(User newDetails)
	{
		User? existingUser = _userRepo.GetUserByEmailOrUsername(newDetails.Username) ?? throw new InvalidOperationException("User does not exist.");

		//if (newDetails.Username is null)
		//{
		//	throw new ArgumentException("Username is empty.");
		//}

		//if (newDetails.Password is null)
		//{
		//	throw new ArgumentException("Password is empty.");
		//}

		//if (newDetails.Email is null)
		//{
		//	throw new ArgumentException("Email is empty.");
		//}

		//if (!MailAddress.TryCreate(newDetails.Email, out _))
		//{
		//	throw new ArgumentException("Email is invalid.");
		//}

		_userRepo.UpdateDetails(existingUser, newDetails);
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
