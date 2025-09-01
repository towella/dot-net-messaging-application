using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;
using DotNetMessagingApplication.Server.Services;
using Moq;

namespace DotNetMessagingApplication.Tests.Services;

public class AccountServiceTests
{
	List<User> testUsers;
	Mock<IUserRepository> _mockUserRepo;
	AccountService _accountService;

	[SetUp]
	public void Setup()
	{
		_mockUserRepo = new Mock<IUserRepository>();

		testUsers = new List<User>
		{
			new User
			{
				Username = "test",
				Password = "password",
				Email = "test@test.com",
			},
		};

		_mockUserRepo.Setup(m => m.AddUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
						.Callback<string, string, string, string>((u, e, pw, pr) => testUsers.Add(new User
						{
							Username = u,
							Password = pw,
							Email = e,
							Pronouns = pr
						}));

		_accountService = new AccountService(_mockUserRepo.Object);
	}

	[Test]
	public void AddUser_WhenEmailIsInvalid_ThrowsException()
	{
		var exception = Assert.Throws<ArgumentException>(() => _accountService.AddUser("u", "invalid", "p", "pr"));
		Assert.That(exception.Message, Is.EqualTo("Email was invalid."));
	}

	[Test]
	public void AddUser_WhenDetailsAreValid_Success()
	{
		var userCount = testUsers.Count;
		Assert.DoesNotThrow(() => _accountService.AddUser("u", "test@email.com", "pw", "pr"));
		Assert.That(testUsers, Has.Count.EqualTo(userCount + 1));
	}
}
