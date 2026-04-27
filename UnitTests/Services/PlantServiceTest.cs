using Entities.plant;
using Grpc.Core;
using GrpcService;
using GrpcService.Services;
using Moq;
using NUnit.Framework;
using RepositoryContracts;
using TemperatureScale = GrpcService.TemperatureScale;

namespace UnitTests.Services;


[TestFixture]
[TestOf(typeof(PlantService))]
public class PlantServiceTest
{
    private Mock<IPlantRepository> _mockRepository;
    private PlantService _plantService;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<IPlantRepository>();
        _plantService = new PlantService(_mockRepository.Object);
    }

    // Create Happy Scenario (plant added)
    [Test]
    public async Task Create_ShouldReturnPlantResponse_WhenPlantIsCreated()
    {
        // Arrange
        var request = new CreatePlantRequest
        {
            Username = "testUser",
            Name = "Test Plant",
            OptimalTemperature = 25.0,
            OptimalAirHumidity = 60.0,
            OptimalSoilHumidity = 40.0,
            OptimalLightIntensity = 1000.0,
            TemperatureScale = TemperatureScale.C
        };

        var plant = new Plant
        {
            Name = request.Name,
            Username = request.Username,
            OptimalTemperature = request.OptimalTemperature,
            OptimalAirHumidity = request.OptimalAirHumidity,
            OptimalSoilHumidity = request.OptimalSoilHumidity,
            OptimalLightIntensity = request.OptimalLightIntensity,
            Scale = (Entities.plant.TemperatureScale)(int)request.TemperatureScale
        };

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Plant>()))
            .ReturnsAsync(plant);

        // Act
        var result = await _plantService.Create(request, null);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Test Plant"));
        Assert.That(result.OptimalTemperature, Is.EqualTo(25.0));
        Assert.That(result.OptimalAirHumidity, Is.EqualTo(60.0));
        Assert.That(result.OptimalSoilHumidity, Is.EqualTo(40.0));
        Assert.That(result.OptimalLightIntensity, Is.EqualTo(1000.0));
        Assert.That(result.TemperatureScale, Is.EqualTo(TemperatureScale.C));

        // Verify repository was called correctly
        _mockRepository.Verify(r => r.CreateAsync(It.Is<Plant>(p =>
            p.Username == request.Username &&
            p.Name == request.Name &&
            p.OptimalTemperature == request.OptimalTemperature &&
            p.OptimalAirHumidity == request.OptimalAirHumidity &&
            p.OptimalSoilHumidity == request.OptimalSoilHumidity &&
            p.OptimalLightIntensity == request.OptimalLightIntensity
        )), Times.Once);
    }

    // Create Unhappy Scenario (plant creation fails)
    [Test]
    public async Task Create_ShouldThrowException_WhenPlantCreationFails()
    {
        // Arrange
        var request = new CreatePlantRequest
        {
            Username = "testUser",
            Name = "Test Plant",
            OptimalTemperature = 25.0,
            OptimalAirHumidity = 60.0,
            OptimalSoilHumidity = 40.0,
            OptimalLightIntensity = 1000.0,
            TemperatureScale = TemperatureScale.C
        };

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Plant>()))
            .ThrowsAsync(new InvalidOperationException("Failed to create plant"));
    }

    // Delete Happy Scenario (plant deleted)
    [Test]
    public async Task Delete_ShouldReturnEmpty_WhenPlantIsDeleted()
    {
        // Arrange
        var request = new DeletePlantRequest
        {
            Username = "testUser",
            PlantMAC = "12345"
        };

        _mockRepository.Setup(r => r.DeleteAsync(request.Username, request.PlantMAC))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _plantService.Delete(request, null);

        // Assert
        Assert.That(result, Is.InstanceOf<Google.Protobuf.WellKnownTypes.Empty>());

        // Verify repository was called correctly
        _mockRepository.Verify(r => r.DeleteAsync(request.Username, request.PlantMAC), Times.Once);
    }

// Update Happy Scenario (plant updated successfully)
    [Test]
    public async Task Update_ShouldReturnEmpty_WhenPlantIsUpdated()
    {
        // Arrange
        var request = new UpdatePlantRequest
        {
            Username = "testUser",
            PlantMAC = "12345",
            Name = "Updated Test Plant",
            OptimalTemperature = 28.0,
            OptimalAirHumidity = 65.0,
            OptimalSoilHumidity = 45.0,
            OptimalLightIntensity = 1200.0,
            TemperatureScale = TemperatureScale.C
        };

        var existingPlant = new Plant
        {
            Username = request.Username,
            MAC = request.PlantMAC,
            Name = "Old Test Plant",
            OptimalTemperature = 27.0,
            OptimalAirHumidity = 64.0,
            OptimalSoilHumidity = 44.0,
            OptimalLightIntensity = 1199.0
        };

        _mockRepository.Setup(r => r.GetPlantAsync(request.Username, request.PlantMAC))
            .ReturnsAsync(existingPlant);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Plant>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _plantService.Update(request, null);

        // Assert
        Assert.That(result, Is.InstanceOf<Google.Protobuf.WellKnownTypes.Empty>());

        // Verify repository was called correctly
        _mockRepository.Verify(r => r.GetPlantAsync(request.Username, request.PlantMAC), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<Plant>(p =>
            p.Username == request.Username &&
            p.MAC == request.PlantMAC &&
            p.Name == request.Name &&
            p.OptimalTemperature == request.OptimalTemperature &&
            p.OptimalAirHumidity == request.OptimalAirHumidity &&
            p.OptimalSoilHumidity == request.OptimalSoilHumidity &&
            p.OptimalLightIntensity == request.OptimalLightIntensity
        )), Times.Once);
    }

// Update Unhappy Scenario (plant update fails)
    [Test]
    public async Task Update_ShouldThrowException_WhenPlantUpdateFails()
    {
        // Arrange
        var request = new UpdatePlantRequest
        {
            Username = "testUser",
            PlantMAC = "12345",
            Name = "Updated Test Plant",
            OptimalTemperature = 28.0,
            OptimalAirHumidity = 65.0,
            OptimalSoilHumidity = 45.0,
            OptimalLightIntensity = 1200.0,
            TemperatureScale = TemperatureScale.C
        };

        _mockRepository.Setup(r => r.GetPlantAsync(request.Username, request.PlantMAC))
            .ThrowsAsync(new InvalidOperationException("Plant not found"));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _plantService.Update(request, null));

        // Verify repository was called correctly
        _mockRepository.Verify(r => r.GetPlantAsync(request.Username, request.PlantMAC), Times.Once);
    }

    // Get Happy Scenario (plant retrieved successfully)
    [Test]
    public async Task Get_ShouldReturnPlantResponse_WhenPlantExists()
    {
        // Arrange
        var request = new GetPlantRequest
        {
            Username = "testUser",
            PlantMAC = "12345",
            Number = 10  // Assuming limit is handled by the repository method
        };

        var plant = new Plant
        {
            MAC = request.PlantMAC,
            Name = "Test Plant",
            Username = request.Username,
            OptimalTemperature = 25.0,
            OptimalAirHumidity = 60.0,
            OptimalSoilHumidity = 40.0,
            OptimalLightIntensity = 1000.0,
            Scale = (Entities.plant.TemperatureScale)(int)request.Number
        };

        _mockRepository.Setup(r => r.GetPlantAsync(request.Username, request.PlantMAC))
            .ReturnsAsync(plant);

        // Act
        var result = await _plantService.Get(request, null);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.PlantMAC, Is.EqualTo("12345"));
        Assert.That(result.Name, Is.EqualTo("Test Plant"));
        Assert.That(result.OptimalTemperature, Is.EqualTo(25.0));
        Assert.That(result.OptimalAirHumidity, Is.EqualTo(60.0));
        Assert.That(result.OptimalSoilHumidity, Is.EqualTo(40.0));
        Assert.That(result.OptimalLightIntensity, Is.EqualTo(1000.0));

        // Verify repository was called correctly
        _mockRepository.Verify(r => r.GetPlantAsync(request.Username, request.PlantMAC), Times.Once);
    }

    // Get Unhappy Scenario (plant retrieval fails)
    [Test]
    public async Task Get_ShouldThrowException_WhenPlantDoesNotExist()
    {
        // Arrange
        var request = new GetPlantRequest
        {
            Username = "testUser",
            PlantMAC = "12345"
        };

        _mockRepository.Setup(r => r.GetPlantAsync(request.Username, request.PlantMAC))
            .ThrowsAsync(new InvalidOperationException("Plant not found"));

        // Act & Assert
        Assert.ThrowsAsync<RpcException>(async () =>
            await _plantService.Get(request, null));

        // Verify repository was called correctly
        _mockRepository.Verify(r => r.GetPlantAsync(request.Username, request.PlantMAC), Times.Once);
    }
    
}
