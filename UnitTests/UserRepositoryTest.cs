using Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using EFC.Repositories;         
using EFC.DataAccess; 

namespace UnitTests
{
    [TestFixture]
    public class UserRepositoryEfcTests
    {
        private Mock<PlantifyContext> _contextMock;
        private Mock<DbSet<User>> _dbSetMock;
        private UserRepository _repository;

        [SetUp]
        public void Setup()
        {
            var users = new List<User>();

            _dbSetMock = CreateDbSetMock(Enumerable.Empty<User>().AsQueryable());
            _contextMock = new Mock<PlantifyContext>();
            _contextMock.Setup(c => c.Users).Returns(_dbSetMock.Object);

            _repository = new UserRepository(_contextMock.Object);
        }

        // ── helpers ──────────────────────────────────────────────────────────

        private static Mock<DbSet<T>> CreateDbSetMock<T>(IQueryable<T> data)
            where T : class
        {
            var enumerator = data.GetEnumerator();
            var mock = new Mock<DbSet<T>>();
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(enumerator);
            mock.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(enumerator));
            return mock;
        }

        private Mock<DbSet<User>> SetupUsersDbSet(IEnumerable<User> seed)
        {
            var queryable = seed.AsQueryable();
            var mock = CreateDbSetMock(queryable);
            _contextMock.Setup(c => c.Users).Returns(mock.Object);
            return mock;
        }

        // ── CreateAsync ───────────────────────────────────────────────────────

        [Test]
        public async Task CreateAsync_ValidUser_AddsAndSaves()
        {
            // Arrange
            var user = new User
            {
                Name = "Bianca",
                Username = "mace",
                Password = "bianca123",
                Email = "bianca.mace@test.com"
            };

            // Act
            var result = await _repository.CreateAsync(user);

            // Assert
            _dbSetMock.Verify(d => d.Add(It.Is<User>(u =>
                u.Username == user.Username &&
                u.Email == user.Email)), Times.Once);

            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(user.Username));
            Assert.That(result.Email, Is.EqualTo(user.Email));
        }

        [Test]
        public void CreateAsync_UserAlreadyExists_ThrowsInvalidOperationException()
        {
            // Arrange
            var existing = new User
            {
                Name = "Bianca",
                Username = "mace",
                Password = "bianca123",
                Email = "bianca.mace@test.com"
            };
            SetupUsersDbSet(new[] { existing });

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _repository.CreateAsync(existing));

            _dbSetMock.Verify(d => d.Add(It.IsAny<User>()), Times.Never);
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        // ── GetByEmailAsync ───────────────────────────────────────────────────

        [Test]
        public async Task GetByEmailAsync_UserExists_ReturnsMappedUser()
        {
            // Arrange
            var email = "bianca.mace@test.com";
            SetupUsersDbSet(new[]
            {
                new User { Name = "Bianca", Username = "mace", Password = "pass", Email = email }
            });

            // Act
            var result = await _repository.GetByEmailAsync(email);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.Username, Is.EqualTo("mace"));
        }

        [Test]
        public void GetByEmailAsync_UserNotFound_ThrowsInvalidOperationException()
        {
            // Arrange — empty DbSet (default from SetUp)

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _repository.GetByEmailAsync("missing@example.com"));
        }

        // ── GetByUsernameAsync ────────────────────────────────────────────────

        [Test]
        public async Task GetByUsernameAsync_UserExists_ReturnsMappedUser()
        {
            // Arrange
            var username = "mace";
            SetupUsersDbSet(new[]
            {
                new User { Name = "Bianca", Username = username, Password = "pass", Email = "bianca.mace@test.com" }
            });

            // Act
            var result = await _repository.GetByUsernameAsync(username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(username));
        }

        [Test]
        public void GetByUsernameAsync_UserNotFound_ThrowsInvalidOperationException()
        {
            // Arrange — empty DbSet (default from SetUp)

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _repository.GetByUsernameAsync("nobody"));
        }

        // ── UpdateAsync ───────────────────────────────────────────────────────

        [Test]
        public async Task UpdateAsync_UserExists_MutatesFieldsAndSaves()
        {
            // Arrange
            var existing = new User
            {
                Name = "Bianca",
                Username = "mace",
                Password = "oldpass",
                Email = "bianca.mace@test.com"
            };
            SetupUsersDbSet(new[] { existing });

            var updated = new User
            {
                Name = "Bianca Updated",
                Username = "mace",
                Password = "newpass",
                Email = "bianca.mace@test.com"
            };

            // Act
            await _repository.UpdateAsync(updated);

            // Assert — entity in the set has been mutated
            Assert.That(existing.Name, Is.EqualTo(updated.Name));
            Assert.That(existing.Password, Is.EqualTo(updated.Password));

            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public void UpdateAsync_UserNotFound_ThrowsInvalidOperationException()
        {
            // Arrange — empty DbSet (default from SetUp)

            var user = new User
            {
                Name = "Ghost",
                Username = "ghost",
                Password = "pass",
                Email = "ghost@test.com"
            };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _repository.UpdateAsync(user));

            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        // ── DeleteAsync ───────────────────────────────────────────────────────

        [Test]
        public async Task DeleteAsync_UserExists_RemovesAndSaves()
        {
            // Arrange
            var username = "mace";
            var existing = new User
            {
                Name = "Bianca",
                Username = username,
                Password = "pass",
                Email = "bianca.mace@test.com"
            };
            SetupUsersDbSet(new[] { existing });

            // Act
            await _repository.DeleteAsync(username);

            // Assert
            _dbSetMock.Verify(d => d.Remove(It.Is<User>(u => u.Username == username)), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_UserNotFound_DoesNothing()
        {
            // Arrange — empty DbSet (default from SetUp)

            // Act — should not throw
            await _repository.DeleteAsync("nobody");

            // Assert
            _dbSetMock.Verify(d => d.Remove(It.IsAny<User>()), Times.Never);
            _contextMock.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        // ── GetManyAsync ──────────────────────────────────────────────────────

        [Test]
        public async Task GetManyAsync_ReturnsAllUsers()
        {
            // Arrange
            SetupUsersDbSet(new[]
            {
                new User { Name = "Bianca", Username = "mace", Password = "pass1", Email = "bianca@example.com" },
                new User { Name = "Bianca", Username = "mace", Password = "pass2", Email = "bianca@example.com" }
            });

            // Act
            var result = _repository.GetMany().ToList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Username, Is.EqualTo("mace"));
            Assert.That(result[1].Username, Is.EqualTo("mace"));
        }
    }

    // ── Async helpers required to mock IAsyncEnumerable on DbSet ─────────────

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
        public T Current => _inner.Current;
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
        public ValueTask DisposeAsync() { _inner.Dispose(); return default; }
    }
}