using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.plant;

public class Plant
{
    //MAC address of the arduino
    [Key] public string MAC {get; set;} = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    //EFC
    public required string Username {get; set;}

    [ForeignKey(nameof(Username))] public User Owner { get; set; } = null!;
    
    public double OptimalTemperature { get; set; }
    public double OptimalAirHumidity { get; set; }
    public double OptimalSoilHumidity { get; set; }
    public double OptimalLightIntensity { get; set; }
    public ICollection<Temperature> Temperatures { get; set; } = new List<Temperature>();
    public ICollection<WaterIntake> WaterIntakes { get; set; } = new List<WaterIntake>();
    public ICollection<SoilHumidity> SoilHumidities { get; set; } = new List<SoilHumidity>();
    public ICollection<AirHumidity> AirHumidities { get; set; } = new List<AirHumidity>();
    public ICollection<LightIntensity> LightIntensities { get; set; } = new List<LightIntensity>();
}