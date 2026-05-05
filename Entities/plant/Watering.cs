using System.Data;

namespace Entities.plant;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Watering
{
    [Key] public int Id {get; set;}
    public double PumpTimeInSeconds { get; set; } = 0;
    public DateTime LastWaterTime { get; set; }
    public DateTime PredictedFutureWaterTime { get; set; }

    public double WaterLevel { get; set; } = 0;
    //EFC
    public string PlantMAC { get; set; } = string.Empty;
    
    [ForeignKey(nameof(PlantMAC))]
    public Plant Plant { get; set; } = null!; //For EFC
}