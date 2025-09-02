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
										old.Email = u.Email;
										old.Phone = u.Phone;
										old.Pronouns = u.Pronouns;
										old.Bio = u.Bio;
									});

		_mockContext.Setup(c => c.SaveChanges()).Verifiable();

		_userRepository = new UserRepository(_mockContext.Object);
	}

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


	/*
	 	User? GetUserByPasswordAndEmailOrUsername(string emailOrUsername, string password);

	User? GetUserByEmailOrUsername(string emailOrUsername);

	void AddUser(string username, string email, string password, string pronouns);

	void UpdateDetails(User oldDetails, User newDetails);
	
	 */

}
