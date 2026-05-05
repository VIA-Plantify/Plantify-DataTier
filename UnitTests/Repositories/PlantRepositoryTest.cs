using EFC.DataAccess;
using EFC.Repositories;
using RepositoryContracts;
using Entities.plant;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Repositories;

[TestFixture]
[TestOf(typeof(PlantRepository))]
public class PlantRepositoryTest
{
    private IPlantRepository _plantRepository;
    private PlantifyContext _context;

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<PlantifyContext>()
            .UseInMemoryDatabase(databaseName: "PlantRepositoryTests")
            .Options;

        _context = new PlantifyContext(options);
        _plantRepository = new PlantRepository(_context);
    }

    [Test]
    public async Task CreateAsync_ShouldCreatePlant()
    {
        // Arrange
        var plant = new Plant
        {
            MAC = "123456",
            Name = "Test Plant",
            Username = "testuser",
            OptimalTemperature = 20.0,
            OptimalAirHumidity = 50.0,
            OptimalSoilHumidity = 60.0,
            OptimalLightIntensity = 1000
        };

        // Act
        var createdPlant = await _plantRepository.CreateAsync(plant);

        // Assert
        Assert.That(createdPlant, Is.Not.Null);
        Assert.That(createdPlant.MAC, Is.EqualTo(plant.MAC));
        Assert.That(createdPlant.Name, Is.EqualTo(plant.Name));
    }

    [Test]
    public async Task GetPlantAsync_ShouldReturnPlant()
    {
        // Arrange
        var plant = new Plant
        {
            MAC = "123456",
            Name = "Test Plant",
            Username = "testuser",
            OptimalTemperature = 20.0,
            OptimalAirHumidity = 50.0,
            OptimalSoilHumidity = 60.0,
            OptimalLightIntensity = 1000
        };

        await _plantRepository.CreateAsync(plant);

        // Act
        var retrievedPlant = await _plantRepository.GetPlantAsync("testuser", "123456", null);

        // Assert
        Assert.That(retrievedPlant, Is.Not.Null);
        Assert.That(retrievedPlant.MAC, Is.EqualTo(plant.MAC));
    }

    [Test]
    public async Task DeleteAsync_ShouldDeletePlant()
    {
        // Arrange
        var plant = new Plant
        {
            MAC = "123456",
            Name = "Test Plant",
            Username = "testuser",
            OptimalTemperature = 20.0,
            OptimalAirHumidity = 50.0,
            OptimalSoilHumidity = 60.0,
            OptimalLightIntensity = 1000
        };

        await _plantRepository.CreateAsync(plant);

        // Act
        await _plantRepository.DeleteAsync("testuser", "123456");

        // Assert
        var deletedPlant = await _context.Plants.FirstOrDefaultAsync(p => p.MAC == "123456");
        Assert.That(deletedPlant, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdatePlant()
    {
        // Arrange
        var plant = new Plant
        {
            MAC = "123456",
            Name = "Test Plant",
            Username = "testuser",
            OptimalTemperature = 20.0,
            OptimalAirHumidity = 50.0,
            OptimalSoilHumidity = 60.0,
            OptimalLightIntensity = 1000
        };

        await _plantRepository.CreateAsync(plant);

        var updatedPlant = new Plant
        {
            MAC = "123456",
            Name = "Updated Test Plant",
            Username = "testuser",
            OptimalTemperature = 25.0,
            OptimalAirHumidity = 55.0,
            OptimalSoilHumidity = 65.0,
            OptimalLightIntensity = 1100
        };

        // Act
        await _plantRepository.UpdateAsync(updatedPlant);

        // Assert
        var retrievedPlant = await _context.Plants.FirstOrDefaultAsync(p => p.MAC == "123456");
        Assert.That(retrievedPlant, Is.Not.Null);
        Assert.That(retrievedPlant.Name, Is.EqualTo(updatedPlant.Name));
    }

    [Test]
    public async Task CreateAsync_ShouldThrowException_WhenPlantExists()
    {
        // Arrange
        var plant = new Plant
        {
            MAC = "123456",
            Name = "Test Plant",
            Username = "testuser",
            OptimalTemperature = 20.0,
            OptimalAirHumidity = 50.0,
            OptimalSoilHumidity = 60.0,
            OptimalLightIntensity = 1000
        };

        await _plantRepository.CreateAsync(plant);

        var duplicatePlant = new Plant
        {
            MAC = "123456",
            Name = "Duplicate Test Plant",
            Username = "testuser",
            OptimalTemperature = 20.0,
            OptimalAirHumidity = 50.0,
            OptimalSoilHumidity = 60.0,
            OptimalLightIntensity = 1000
        };

        // Act & Assert
        var exception =  Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _plantRepository.CreateAsync(duplicatePlant);
        });

        Assert.That(exception.Message, Is.EqualTo($"Plant with MAC {duplicatePlant.MAC} already exists."));
    }

    [Test]
    public async Task GetPlantAsync_ShouldThrowException_WhenPlantNotFound()
    {
        // Arrange
        var nonExistentMAC = "987654";

        // Act & Assert
        var exception =  Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _plantRepository.GetPlantAsync("testuser", nonExistentMAC, null);
        });

        Assert.That(exception.Message, Is.EqualTo($"Plant with MAC '{nonExistentMAC}' for user 'testuser' not found."));
    }
    

    [Test]
    public async Task UpdateAsync_ShouldThrowException_WhenPlantNotFound()
    {
        // Arrange
        var plant = new Plant
        {
            MAC = "123456",
            Name = "Nonexistent Test Plant",
            Username = "testuser",
            OptimalTemperature = 20.0,
            OptimalAirHumidity = 50.0,
            OptimalSoilHumidity = 60.0,
            OptimalLightIntensity = 1000
        };

        // Act & Assert
        var exception =  Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _plantRepository.UpdateAsync(plant);
        });

        Assert.That(exception.Message, Is.EqualTo($"Plant with MAC '{plant.MAC}' for user 'testuser' not found."));
    }

    [Test]
    public async Task GetMany_ShouldReturnAllPlants()
    {
        // Arrange
        var plant1 = new Plant
        {
            MAC = "123456",
            Name = "Test Plant 1",
            Username = "testuser",
            OptimalTemperature = 20.0,
            OptimalAirHumidity = 50.0,
            OptimalSoilHumidity = 60.0,
            OptimalLightIntensity = 1000
        };

        var plant2 = new Plant
        {
            MAC = "654321",
            Name = "Test Plant 2",
            Username = "testuser",
            OptimalTemperature = 25.0,
            OptimalAirHumidity = 55.0,
            OptimalSoilHumidity = 65.0,
            OptimalLightIntensity = 1100
        };

        await _plantRepository.CreateAsync(plant1);
        await _plantRepository.CreateAsync(plant2);

        // Act
        var plants = await _plantRepository.GetMany("testuser", null).ToListAsync();

        // Assert
        Assert.That(plants, Has.Count.EqualTo(2));
        Assert.That(plants.Any(p => p.MAC == "123456" && p.Name == "Test Plant 1"), Is.True);
        Assert.That(plants.Any(p => p.MAC == "654321" && p.Name == "Test Plant 2"), Is.True);
    }
    
    //FIX TEST
    /*[Test]
    public async Task GetPlantAsync_ShouldLimitReadings_WhenNumberProvided()
    {
        var plant = new Plant
        {
            MAC = "123456",
            Name = "Test Plant",
            Username = "testuser",
            Temperatures = Enumerable.Range(1, 20)
                .Select(i => new Temperature { Value = i })
                .ToList(),
            AirHumidities = Enumerable.Range(1, 20)
                .Select(i => new AirHumidity { Value = i })
                .ToList(),
            SoilHumidities = Enumerable.Range(1, 20)
                .Select(i => new SoilHumidity { Value = i })
                .ToList(),
            LightIntensities = Enumerable.Range(1, 20)
                .Select(i => new LightIntensity { Value = i })
                .ToList()
        };

        await _plantRepository.CreateAsync(plant);

        var result = await _plantRepository.GetPlantAsync("testuser", "123456", 5);

        Assert.That(result.Temperatures, Has.Count.EqualTo(5));
        Assert.That(result.AirHumidities, Has.Count.EqualTo(5));
        Assert.That(result.SoilHumidities, Has.Count.EqualTo(5));
        Assert.That(result.LightIntensities, Has.Count.EqualTo(5));
    }
    [Test]
    public async Task GetPlantAsync_ShouldDefaultToTenReadings_WhenNumberIsNull()
    {
        var plant = new Plant
        {
            MAC = "123456",
            Name = "Test Plant",
            Username = "testuser",
            Temperatures = Enumerable.Range(1, 20)
                .Select(i => new Temperature { Value = i })
                .ToList()
        };

        await _plantRepository.CreateAsync(plant);

        var result = await _plantRepository.GetPlantAsync("testuser", "123456", null);

        Assert.That(result.Temperatures, Has.Count.EqualTo(10));
    }
    */
    
}