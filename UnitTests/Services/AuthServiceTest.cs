using Entities;
using Grpc.Core;
using GrpcService;
using GrpcService.Services;
using Moq;
using RepositoryContracts;

namespace UnitTests.Services;

[TestFixture]
public class AuthServiceTest
{
    private Mock<IUserRepository> _repoMock;
    private AuthService _service;
    
    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IUserRepository>();
        _service = new AuthService(_repoMock.Object);
    }
    
    //Login by Username (Happy scenario)
    [Test]
    public async Task Login_ShouldReturnUserResponse_WhenUsernameIsProvidedAndUserExists()
    {
        //Arrange
        var request = new AuthRequest
        {
            Username = "teo_st"
        };

        var user = new User
        {
            Name = "Teodora",
            Username = "teo_st",
            Password = "1234",
            Email = "test@email.com"
        };
        
        _repoMock.Setup(r => r.GetByUsernameAsync("teo_st"))
            .ReturnsAsync(user);
        
        //Act
        var result = await _service.Login(request, null);
        
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("teo_st"));
        Assert.That(result.Email, Is.EqualTo("test@email.com"));
        Assert.That(result.Name, Is.EqualTo("Teodora"));
        Assert.That(result.Password, Is.EqualTo("1234"));
        
        _repoMock.Verify(r => r.GetByUsernameAsync("teo_st"), Times.Once);
    }
    
    //Login by Username Unhappy Scenario
    [Test]
    public void Login_ShouldThrowRpcException_WhenUsernameDoesNotExist()
    {
        //Arrange
        var request = new AuthRequest
        {
            Username = "missingUser"
        };
 
        _repoMock.Setup(r => r.GetByUsernameAsync("missingUser"))
            .ThrowsAsync(new InvalidOperationException("User not found"));
 
        //Act
        var exception = Assert.ThrowsAsync<RpcException>(() =>
            _service.Login(request, null));
 
        //Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.NotFound));
        Assert.That(exception.Status.Detail, Does.Contain("User not found"));
    }
    
    //Login by Email Happy Scenario
    [Test]
    public async Task Login_ShouldReturnUserResponse_WhenEmailIsProvidedAndUserExists()
    {
        //Arrange
        var request = new AuthRequest
        {
            Email = "test@email.com"
        };
 
        var user = new User
        {
            Name = "Teodora",
            Username = "teo_st",
            Password = "1234",
            Email = "test@email.com"
        };
 
        _repoMock.Setup(r => r.GetByEmailAsync("test@email.com"))
            .ReturnsAsync(user);
 
        //Act
        var result = await _service.Login(request, null);
 
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo("test@email.com"));
        Assert.That(result.Username, Is.EqualTo("teo_st"));
        Assert.That(result.Name, Is.EqualTo("Teodora"));
        Assert.That(result.Password, Is.EqualTo("1234"));
 
        _repoMock.Verify(r => r.GetByEmailAsync("test@email.com"), Times.Once);
    }
    
    //Login by Email Unhappy Scenario
    [Test]
    public void Login_ShouldThrowRpcException_WhenEmailDoesNotExist()
    {
        //Arrange
        var request = new AuthRequest
        {
            Email = "missing@test.com"
        };
 
        _repoMock.Setup(r => r.GetByEmailAsync("missing@test.com"))
            .ThrowsAsync(new InvalidOperationException("User not found"));
 
        //Act
        var exception = Assert.ThrowsAsync<RpcException>(() =>
            _service.Login(request, null));
 
        //Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.NotFound));
        Assert.That(exception.Status.Detail, Does.Contain("User not found"));
    }
    
    //Login with Email as Fallback Happy Scenario
    [Test]
    public async Task Login_ShouldUseEmail_WhenUsernameIsEmptyAndEmailIsProvided()
    {
        //Arrange
        var request = new AuthRequest
        {
            Username = "",
            Email = "test@email.com"
        };
 
        var user = new User
        {
            Name = "Teodora",
            Username = "teo_st",
            Password = "1234",
            Email = "test@email.com"
        };
 
        _repoMock.Setup(r => r.GetByEmailAsync("test@email.com"))
            .ReturnsAsync(user);
 
        //Act
        var result = await _service.Login(request, null);
 
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo("test@email.com"));
 
        _repoMock.Verify(r => r.GetByEmailAsync("test@email.com"), Times.Once);
        _repoMock.Verify(r => r.GetByUsernameAsync(It.IsAny<string>()), Times.Never);
    }
    
    //Login prefers Username over Email
    [Test]
    public async Task Login_ShouldUseUsername_WhenBothUsernameAndEmailAreProvided()
    {
        //Arrange
        var request = new AuthRequest
        {
            Username = "teo_st",
            Email = "test@email.com"
        };
 
        var user = new User
        {
            Name = "Teodora",
            Username = "teo_st",
            Password = "1234",
            Email = "test@email.com"
        };
 
        _repoMock.Setup(r => r.GetByUsernameAsync("teo_st"))
            .ReturnsAsync(user);
 
        //Act
        var result = await _service.Login(request, null);
 
        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("teo_st"));
 
        _repoMock.Verify(r => r.GetByUsernameAsync("teo_st"), Times.Once);
        _repoMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never);
    }
}