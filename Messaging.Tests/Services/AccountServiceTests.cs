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

		_accountService.UpdateDetails(newDetails);

		Assert.DoesNotThrow(() => _accountService.UpdateDetails(newDetails));
		_mockUserRepo.Verify(m => m.UpdateDetails(It.IsAny<User>()), Times.Once);
	}

	//[TestCase("test", null, "new@email.com", "she/her", "Password is empty.", Description = "Empty password check")]
	//[TestCase("test", "newPassword", null, "she/her", "Email is empty.", Description = "Empty email check")]
	//[TestCase("test", "newPassword", "invalid email", "she/her", "Email is invalid.", Description = "Invalid email check")]
	//public void UpdateDetails_WhenInvalidDetails_ThrowsArgumentException(string? username, string? password, string? email, string? pronouns, string expectedMessage)
	//{
	//	_mockUserRepo.Setup(m => m.GetUserByEmailOrUsername(It.Is<string>(x => x == "test" || x == "test@test.com"))).Returns(testUsers[0]);

	//	User newDetails = new User()
	//	{
	//		Username = username ?? "newUsername",
	//		Password = password!,
	//		Email = email!,
	//		Pronouns = pronouns,
	//	};

	//	ArgumentException ex = Assert.Throws<ArgumentException>(() => _accountService.UpdateDetails(newDetails));
	//	Assert.That(ex.Message, Is.EqualTo(expectedMessage));
	//}

	#endregion
}
