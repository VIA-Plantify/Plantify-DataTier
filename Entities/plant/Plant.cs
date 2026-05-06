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
    public TemperatureScale Scale { get; set; } = TemperatureScale.C;
    [ForeignKey(nameof(Username))] public User Owner { get; set; } = null!;
    
    public double OptimalTemperature { get; set; }
    public double OptimalAirHumidity { get; set; }
    public double OptimalSoilHumidity { get; set; }
    public double OptimalLightIntensity { get; set; }
    
    
    //FOR EFC
    public ICollection<Watering> Waterings { get; set; } = new List<Watering>();
    public ICollection<SensorData> SensorDatas { get; set; } = new List<SensorData>();
    
}