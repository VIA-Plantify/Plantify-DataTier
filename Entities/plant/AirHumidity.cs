namespace Entities.plant;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AirHumidity
{
    [Key] public int Id {get; set;}
    public double? Value { get; set; }
    //EFC
    public string PlantMAC { get; set; } = string.Empty;
    
    [ForeignKey(nameof(PlantMAC))]
    public Plant Plant { get; set; } = null!; //For EFC
}