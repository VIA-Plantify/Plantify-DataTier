using Entities;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService;
using GrpcService.Services;
using MockQueryable;
using Moq;
using RepositoryContracts;

namespace UnitTests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _repoMock;
    private UserService _service;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IUserRepository>();
        _service = new UserService(_repoMock.Object);
    }

    // Create Happy Scenario (user added)
    [Test]
    public async Task Create_ShouldReturnUserResponse_WhenUserIsCreated()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Name = "Carolina",
            Username = "caro",
            Password = "123",
            Email = "caro@test.com"
        };

        var user = new User
        {
            Name = request.Name,
            Username = request.Username,
            Password = request.Password,
            Email = request.Email
        };

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        // Act
        var result = await _service.Create(request, null);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("caro"));
        Assert.That(result.Email, Is.EqualTo("caro@test.com"));

        // Verify repository was called correctly
        _repoMock.Verify(r => r.CreateAsync(It.Is<User>(u =>
            u.Username == request.Username &&
            u.Email == request.Email
        )), Times.Once);
    }

    // Create Unhappy scenario (user already exists)
    [Test]
    public void Create_ShouldThrowRpcException_WhenUserAlreadyExists()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Name = "Carolina",
            Username = "caro",
            Password = "123",
            Email = "caro@test.com"
        };

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ThrowsAsync(new InvalidOperationException("User already exists"));

        // Act
        var exception = Assert.ThrowsAsync<RpcException>(() =>
            _service.Create(request, null));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.AlreadyExists));
    }
    
    // Update Happy Scenario
    [Test]
    public async Task Update_ShouldCallRepositoryUpdate_WhenRequestIsValid()
    {
        // Arrange
        var request = new UpdateUserRequest
        {
            Name = "Updated Name",
            Username = "caro",
            Password = "newpass",
            Email = "updated@test.com"
        };

        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.Update(request, null);

        // Assert
        Assert.That(result, Is.Not.Null);

        _repoMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
            u.Username == request.Username &&
            u.Email == request.Email &&
            u.Name == request.Name &&
            u.Password == request.Password
        )), Times.Once);
    }
    
    // Update Unhappy Scenario
    [Test]
    public void Update_ShouldThrowRpcException_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new UpdateUserRequest
        {
            Name = "Updated Name",
            Username = "missing",
            Password = "123",
            Email = "missing@test.com"
        };

        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .ThrowsAsync(new InvalidOperationException("User not found"));

        // Act
        var exception = Assert.ThrowsAsync<RpcException>(() =>
            _service.Update(request, null));

        // Assert
        Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.NotFound));
    }
    
    // Delete Happy Scenario
    [Test]
    public async Task Delete_ShouldCallRepositoryDelete_WhenUsernameIsProvided()
    {
        // Arrange
        var request = new DeleteUserRequest
        {
            Username = "caro"
        };

        _repoMock.Setup(r => r.DeleteAsync("caro"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.Delete(request, null);

        // Assert
        Assert.That(result, Is.Not.Null);

        _repoMock.Verify(r => r.DeleteAsync("caro"), Times.Once);
    }
    
    // Delete Unhappy Scenario
    [Test]
    public void Delete_ShouldThrowRpcException_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new DeleteUserRequest
        {
            Username = "missing"
        };

        _repoMock.Setup(r => r.DeleteAsync("missing"))
            .ThrowsAsync(new InvalidOperationException("User not found"));

        // Act
        var exception = Assert.ThrowsAsync<RpcException>(() =>
            _service.Delete(request, null));

        // Assert
        Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.NotFound));
    }
    
    // Get by Username Happy Scenario
    [Test]
    public async Task Get_ShouldReturnUser_WhenUsernameIsProvided()
    {
        // Arrange
        var request = new GetUserRequest
        {
            Username = "caro"
        };

        var user = new User
        {
            Name = "Carolina",
            Username = "caro",
            Password = "123",
            Email = "caro@test.com"
        };

        _repoMock.Setup(r => r.GetByUsernameAsync("caro"))
            .ReturnsAsync(user);

        // Act
        var result = await _service.Get(request, null);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("caro"));

        _repoMock.Verify(r => r.GetByUsernameAsync("caro"), Times.Once);
    }
    
    // Get by Username Unhappy Scenario
    [Test]
    public void Get_ShouldThrowRpcException_WhenUsernameDoesNotExist()
    {
        // Arrange
        var request = new GetUserRequest
        {
            Username = "missingUser"
        };

        _repoMock.Setup(r => r.GetByUsernameAsync("missingUser"))
            .ThrowsAsync(new InvalidOperationException("User not found"));

        // Act
        var exception = Assert.ThrowsAsync<RpcException>(() =>
            _service.Get(request, null));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.NotFound));

        // Optional (nice detail)
        Assert.That(exception.Status.Detail, Does.Contain("User not found"));
    }
    
    // Get by Email Happy Scenario
    [Test]
    public async Task Get_ShouldReturnUser_WhenEmailIsProvided()
    {
        // Arrange
        var request = new GetUserRequest
        {
            Email = "caro@test.com"
        };

        var user = new User
        {
            Name = "Carolina",
            Username = "caro",
            Password = "123",
            Email = "caro@test.com"
        };

        _repoMock.Setup(r => r.GetByEmailAsync("caro@test.com"))
            .ReturnsAsync(user);

        // Act
        var result = await _service.Get(request, null);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo("caro@test.com"));

        _repoMock.Verify(r => r.GetByEmailAsync("caro@test.com"), Times.Once);
    }
    // Get by Email UnHappy Scenario
    [Test]
    public void Get_ShouldThrowRpcException_WhenEmailDoesNotExist()
    {
        // Arrange
        var request = new GetUserRequest
        {
            Email = "missing@test.com"
        };

        _repoMock.Setup(r => r.GetByEmailAsync("missing@test.com"))
            .ThrowsAsync(new InvalidOperationException("User not found"));

        // Act
        var exception = Assert.ThrowsAsync<RpcException>(() =>
            _service.Get(request, null));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.NotFound));

        // Optional (extra verification)
        Assert.That(exception.Status.Detail, Does.Contain("User not found"));
    }
    
    // Get Many Happy Scenario
    [Test]
    public async Task GetAll_ShouldReturnUsers_WhenRepositoryReturnsQueryable()
    {
        var users = new List<User>
        {
            new User { Name = "Carolina", Username = "caro", Password = "123", Email = "caro@test.com" },
            new User { Name = "Alex", Username = "alex", Password = "456", Email = "alex@test.com" }
        };

        var mockQueryable = users.BuildMock();

        _repoMock.Setup(r => r.GetMany())
            .Returns(mockQueryable);

        var result = await _service.GetAll(new Empty(), null);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Users.Count, Is.EqualTo(2));
        Assert.That(result.Users[0].Username, Is.EqualTo("caro"));
        Assert.That(result.Users[1].Username, Is.EqualTo("alex"));
    }
    
    // Get Many Unhappy Scenario
    [Test]
    public void GetAll_ShouldThrowRpcException_WhenRepositoryThrows()
    {
        // Arrange
        _repoMock.Setup(r => r.GetMany())
            .Throws(new Exception("Database failure"));

        // Act
        var exception = Assert.ThrowsAsync<RpcException>(() =>
            _service.GetAll(new Google.Protobuf.WellKnownTypes.Empty(), null));

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.Internal));
        Assert.That(exception.Status.Detail, Does.Contain("Database failure"));
    }
}