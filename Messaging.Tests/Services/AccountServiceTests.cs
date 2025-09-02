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

		_mockUserRepo.Setup(m => m.GetUserByEmailOrUsername(It.IsAny<string>())).Returns((User?)null);
		_mockUserRepo.Setup(m => m.GetUserByEmailOrUsername(It.Is<string>(s => s == testUsers[0].Username || s == testUsers[0].Email))).Returns(testUsers[0]);

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


	#region GetDetails

	[Test]
	public void GetDetails_WhenUserDoesNotExist_ThrowsException()
	{
		InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => _accountService.GetDetails("minion"));
		Assert.That(ex.Message, Is.EqualTo("User does not exist."));
	}

	[TestCase("test", Description = "Username check.")]
	[TestCase("test@test.com", Description = "Username check.")]
	public void GetDetails_WhenUserExists_ReturnsCorrectDetails(string emailOrUsername)
	{
		User user = _accountService.GetDetails(emailOrUsername);
		Assert.That(user.Username, Is.EqualTo("test"));
		Assert.That(user.Email, Is.EqualTo("test@test.com"));
		Assert.That(user.Password, Is.EqualTo("password"));
	}

	#endregion
}
