using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.plant;

public class Temperature
{
    [Key]
    public int Id { get; set; }
    public double? CurrentTemperature { get; set; }
    public TemperatureScale TemperatureScale { get; set; } = TemperatureScale.C;
    
    //EFC
    
    public string PlantMAC { get; set; } = string.Empty;
    
    [ForeignKey(nameof(PlantMAC))]
    public Plant Plant { get; set; } = null!; //For EFC
    
}