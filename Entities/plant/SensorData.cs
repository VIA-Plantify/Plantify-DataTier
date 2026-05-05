using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.plant;

public class SensorData
{
    [Key] public long Id { get; set; }
    public double Temperature { get; set; } = 0;
    public double AirHumidity { get; set; } = 0;
    public double SoilHumidity { get; set; } = 0;
    public double LightIntensity { get; set; } = 0;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    
    //EFC
    public string PlantMAC { get; set; } = string.Empty;
    
    [ForeignKey(nameof(PlantMAC))]
    public Plant Plant { get; set; } = null!; //For EFC
}