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
    
    public int PlantId { get; set; }
    
    [ForeignKey(nameof(PlantId))]
    public Plant Plant { get; set; } = null!; //For EFC
    
}