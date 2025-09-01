using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;
using DotNetMessagingApplication.Server.Services;
using Moq;

namespace DotNetMessagingApplication.Tests.Services;

[TestFixture]
public class LoginServiceTests
{
	Mock<IUserRepository> _mockUserRepo;
	LoginService _loginService;

	[SetUp]
	public void Setup()
	{
		_mockUserRepo = new Mock<IUserRepository>();
		_loginService = new LoginService(_mockUserRepo.Object);

		User testUser = new User
		{
			Username = "test",
			Password = "password",
			Email = "test@test.com",
		};

		_mockUserRepo.Setup(m => m.GetUserByPasswordAndEmailOrUsername(It.Is<string>(x => x == "test" || x == "test@test.com"), 
																		It.Is<string>(x => x == "password")))
															.Returns(testUser);
	}

	[TestCase("test", "password")]
	[TestCase("test@test.com", "password")]
	public void Login_WhenDetailsAreCorrect_ReturnsTrue(string emailOrUsername, string password)
	{
		bool loggedIn = _loginService.Login(emailOrUsername, password);
		Assert.That(loggedIn);
	}

	[TestCase("test", "wrongpassword")]
	[TestCase("test@test.com", "wrongpassword")]
	[TestCase("minion", "password")]
	[TestCase("minion", "banana")]
	public void Login_WhenDetailsAreIncorrect_ReturnsFalse(string emailOrUsername, string password)
	{
		bool loggedIn = _loginService.Login(emailOrUsername, password);
		Assert.That(loggedIn, Is.False);
	}
}
