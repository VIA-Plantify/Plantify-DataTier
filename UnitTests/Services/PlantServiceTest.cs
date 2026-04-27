using Entities.plant;
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
        
    }
}