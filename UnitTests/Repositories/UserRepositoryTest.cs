using Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using EFC.Repositories;
using EFC.DataAccess;

namespace UnitTests.Repositories
{
    [TestFixture]
    public class UserRepositoryEfcTests
    {
        private PlantifyContext _context;
        private UserRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PlantifyContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PlantifyContext(options);
            _repository = new UserRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
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
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(user.Username));
            Assert.That(result.Email, Is.EqualTo(user.Email));

            var inDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            Assert.That(inDb, Is.Not.Null);
        }

        [Test]
        public async Task CreateAsync_UserAlreadyExists_ThrowsInvalidOperationException()
        {
            // Arrange
            var existing = new User
            {
                Name = "Bianca",
                Username = "mace",
                Password = "bianca123",
                Email = "bianca.mace@test.com"
            };
            await _context.Users.AddAsync(existing);
            await _context.SaveChangesAsync();

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _repository.CreateAsync(existing));

            var count = await _context.Users.CountAsync();
            Assert.That(count, Is.EqualTo(1));
        }

        // ── GetByEmailAsync ───────────────────────────────────────────────────

        [Test]
        public async Task GetByEmailAsync_UserExists_ReturnsMappedUser()
        {
            // Arrange
            var user = new User
            {
                Name = "Bianca",
                Username = "mace",
                Password = "bianca123",
                Email = "bianca.mace@test.com"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByEmailAsync(user.Email);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Username, Is.EqualTo(user.Username));
        }

        [Test]
        public void GetByEmailAsync_UserNotFound_ThrowsInvalidOperationException()
        {
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _repository.GetByEmailAsync("missing@example.com"));
        }

        // ── GetByUsernameAsync ────────────────────────────────────────────────

        [Test]
        public async Task GetByUsernameAsync_UserExists_ReturnsMappedUser()
        {
            // Arrange
            var user = new User
            {
                Name = "Bianca",
                Username = "mace",
                Password = "bianca123",
                Email = "bianca.mace@test.com"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUsernameAsync(user.Username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo(user.Username));
            Assert.That(result.Email, Is.EqualTo(user.Email));
        }

        [Test]
        public void GetByUsernameAsync_UserNotFound_ThrowsInvalidOperationException()
        {
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
                Email = "bianca.mace@test.com"
            };
            await _context.Users.AddAsync(existing);
            await _context.SaveChangesAsync();

            var updated = new User
            {
                Name = "Bianca Updated",
                Username = "mace",
                Email = "bianca.mace@test.com"
            };

            // Act
            await _repository.UpdateAsync(updated);

            // Assert
            var inDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == "mace");
            Assert.That(inDb, Is.Not.Null);
            Assert.That(inDb.Name, Is.EqualTo(updated.Name));
        }

        [Test]
        public void UpdateAsync_UserNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
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
        }

        // ── DeleteAsync ───────────────────────────────────────────────────────

        [Test]
        public async Task DeleteAsync_UserExists_RemovesAndSaves()
        {
            // Arrange
            var user = new User
            {
                Name = "Bianca",
                Username = "mace",
                Password = "bianca123",
                Email = "bianca.mace@test.com"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync("mace");

            // Assert
            var inDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == "mace");
            Assert.That(inDb, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_UserNotFound_DoesNothing()
        {
            // Act — should not throw
            await _repository.DeleteAsync("nobody");

            // Assert
            Assert.That(await _context.Users.CountAsync(), Is.EqualTo(0));
        }

        // ── GetMany ───────────────────────────────────────────────────────────

        [Test]
        public async Task GetMany_ReturnsAllUsers()
        {
            // Arrange
            await _context.Users.AddRangeAsync(
                new User { Name = "Bianca", Username = "mace", Password = "pass1", Email = "bianca@test.com" },
                new User { Name = "John",   Username = "johndoe", Password = "pass2", Email = "john@test.com" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = _repository.GetMany().ToList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(u => u.Username == "mace"), Is.True);
            Assert.That(result.Any(u => u.Username == "johndoe"), Is.True);
        }
    }
}