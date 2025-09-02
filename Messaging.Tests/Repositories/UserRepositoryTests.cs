using DotNetMessagingApplication.Server.Data;
using DotNetMessagingApplication.Server.Data.Models;
using DotNetMessagingApplication.Server.Data.Repositories;
using DotNetMessagingApplication.Tests.Utilities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DotNetMessagingApplication.Tests.Repositories;

public class UserRepositoryTests
{
	Mock<MessagingAppContext> _mockContext;
	Mock<DbSet<User>> _mockUserDbSet;

	UserRepository _userRepository;

	[SetUp]
	public void Setup()
	{
		List<User> users = new List<User>
		{
			new User { Username = "test", Password = "test", Email = "test@test.com" },
		};

		_mockUserDbSet = MockDbSetHelper.CreateMockDbSet(users.AsQueryable());

		_mockContext = new Mock<MessagingAppContext>();

		_mockContext.Setup(c => c.Users).Returns(_mockUserDbSet.Object);
		_mockContext.Setup(c => c.Set<User>()).Returns(_mockUserDbSet.Object);
		_mockContext.Setup(c => c.Set<User>()
										.Add(It.IsAny<User>()))
									.Callback<User>(users.Add);

		// update is done by retrieving user by username, not email
		_mockContext.Setup(c => c.Set<User>()
										.Update(It.IsAny<User>()))
									.Callback<User>(u =>
									{
										User old = users.Single(user => user.Username == u.Username);
										old.Username = u.Username;
										old.Password = u.Password;
										old.Email = u.Email;
										old.Phone = u.Phone;
										old.Pronouns = u.Pronouns;
										old.Bio = u.Bio;
									});

		_mockContext.Setup(c => c.SaveChanges()).Verifiable();

		_userRepository = new UserRepository(_mockContext.Object);
	}

	#region GetUserByPasswordAndEmailOrUsername

	// covers both success and failure cases
	[TestCase("test", "test", true, Description = "Correct username check")]
	[TestCase("test@test.com", "test", true, Description = "Correct email check")]
	[TestCase("test", "wrongPassword", false, Description = "Correct username, wrong password check")]
	[TestCase("test@test.com", "wrongPassword", false, Description = "Correct email, wrong password check")]
	[TestCase("duck", "wrongPassword", false, Description = "Wrong details check")]
	public void GetUserByPasswordAndEmailOrUsername_ReturnsExpectedResult(string emailOrUsername, string password, bool shouldFindUser)
	{
		User? user = _userRepository.GetUserByPasswordAndEmailOrUsername(emailOrUsername, password);

		Assert.That(user is not null, Is.EqualTo(shouldFindUser));
	}

	[TestCase("test", true, Description = "Correct username check")]
	[TestCase("test@test.com", true, Description = "Correct email check")]
	[TestCase("duck", false, Description = "Wrong details check")]
	public void GetUserByEmailOrUsername_ReturnsExpectedResult(string emailOrUsername, bool shouldFindUser)
	{
		User? user = _userRepository.GetUserByEmailOrUsername(emailOrUsername);

		Assert.That(user is not null, Is.EqualTo(shouldFindUser));
	}

	#endregion


	#region AddUser

	[Test]
	public void AddUser_WhenEmailAndUsernameAreUnique_Success()
	{
		int userCount = _mockContext.Object.Users.Count();

		_userRepository.AddUser("banana", "minion@banana.com", "banana", "he/him");

		Assert.That(_mockContext.Object.Users.Count(), Is.EqualTo(userCount + 1));
		Assert.That(_mockContext.Object.Users.Last().Username, Is.EqualTo("banana"));
		Assert.That(_mockContext.Object.Users.Last().Email, Is.EqualTo("minion@banana.com"));
		Assert.That(_mockContext.Object.Users.Last().Password, Is.EqualTo("banana"));
		Assert.That(_mockContext.Object.Users.Last().Pronouns, Is.EqualTo("he/him"));
	}

	[Test]
	public void AddUser_WhenEmailIsNotUnique_ThrowsException()
	{
		int userCount = _mockContext.Object.Users.Count();

		ArgumentException ex = Assert.Throws<ArgumentException>(() => _userRepository.AddUser("newUser", "test@test.com", "password", "they/them"));
		Assert.That(_mockContext.Object.Users.Count(), Is.EqualTo(userCount));
	}

	[Test]
	public void AddUser_WhenUsernameIsNotUnique_ThrowsException()
	{
		int userCount = _mockContext.Object.Users.Count();

		ArgumentException ex = Assert.Throws<ArgumentException>(() => _userRepository.AddUser("test", "new@email.com", "password", "they/them"));
		Assert.That(ex.Message, Is.EqualTo("Username or email already exists."));
		Assert.That(_mockContext.Object.Users.Count(), Is.EqualTo(userCount));
	}

	#endregion


	#region UpdateDetails

	[Test]
	public void UpdateDetails_Success()
	{
		User newDetails = new User { Username = "test", Password = "newPassword", Email = "new@email.com", Pronouns = "they/them" };
		_userRepository.UpdateDetails(newDetails);

		Assert.That(_mockContext.Object.Users.First().Password, Is.EqualTo(newDetails.Password));
		Assert.That(_mockContext.Object.Users.First().Email, Is.EqualTo(newDetails.Email));
		Assert.That(_mockContext.Object.Users.First().Pronouns, Is.EqualTo(newDetails.Pronouns));

		_mockContext.Verify(m => m.Set<User>().Update(It.Is<User>(u => u.Username == "test")), Times.Once);
	}

	#endregion
}
