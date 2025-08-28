namespace DotNetMessagingApplication.Server.Services;

public interface ILoginService
{
	bool Login(string emailOrUsername, string password);
}
public class LoginService : ILoginService
{
	public bool Login(string emailOrUsername, string password)
	{
		throw new NotImplementedException();
	}
}
