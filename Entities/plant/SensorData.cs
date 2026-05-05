using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.plant;

public class SensorData
{
    [Key] public long Id { get; set; }
    public double Temperature { get; set; }
    public double AirHumidity { get; set; }
    public double SoilHumidity { get; set; }
    public double LightIntensity { get; set; }
    public DateTime Timestamp { get; set; }
    
    //EFC
    public string PlantMAC { get; set; } = string.Empty;
    
    [ForeignKey(nameof(PlantMAC))]
    public Plant Plant { get; set; } = null!; //For EFC
}