using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;

namespace DotNetMessagingApplication.Server.Services;

public interface IAccountService
{
	void UpdateDetails(User user);

	User GetDetails(string emailOrPassword);
}

public class AccountService(UserRepository userRepo) : IAccountService
{
	readonly UserRepository _userRepo = userRepo;

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
