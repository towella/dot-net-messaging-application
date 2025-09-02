using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;

namespace DotNetMessagingApplication.Server.Services;

public interface ILoginService
{
	bool Login(string emailOrUsername, string password);
}

public class LoginService(IUserRepository userRepo) : ILoginService
{
	readonly IUserRepository _userRepo = userRepo;

	public bool Login(string emailOrUsername, string password)
	{
		User? user = _userRepo.GetUserByPasswordAndEmailOrUsername(emailOrUsername, password);
		
		return user is not null;
	}
}
