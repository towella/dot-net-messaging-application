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

		_mockUserRepo.Setup(m => m.UpdateDetails(It.IsAny<User>())).Verifiable();

		_accountService = new AccountService(_mockUserRepo.Object);
	}

	#region AddUser

	[Test]
	public void AddUser_WhenEmailIsInvalid_ThrowsException()
	{
		ArgumentException exception = Assert.Throws<ArgumentException>(() => _accountService.AddUser("u", "invalid", "p", "pr"));
		Assert.That(exception.Message, Is.EqualTo("Email was invalid."));
	}

	[Test]
	public void AddUser_WhenDetailsAreValid_Success()
	{
		int userCount = testUsers.Count;
		Assert.DoesNotThrow(() => _accountService.AddUser("u", "test@email.com", "pw", "pr"));
		Assert.That(testUsers, Has.Count.EqualTo(userCount + 1));
	}

	#endregion


	#region UpdateDetails

	[Test]
	public void UpdateDetails_WhenValidDetails_Success()
	{
		_mockUserRepo.Setup(m => m.GetUserByEmailOrUsername(It.Is<string>(x => x == "test" || x == "test@test.com"))).Returns(testUsers[0]);
		_mockUserRepo.Setup(m => m.UpdateDetails(It.IsAny<User>())).Verifiable();

		User newDetails = new User()
		{
			Username = testUsers[0].Username,
			Password = testUsers[0].Password,
			Email = testUsers[0].Email,
			Pronouns = "she/her",
		};

		Assert.DoesNotThrow(() => _accountService.UpdateDetails(newDetails));
		_mockUserRepo.Verify(m => m.UpdateDetails(It.Is<User>(u => u.Username == "test")), Times.Once);
	}

	#endregion
}
