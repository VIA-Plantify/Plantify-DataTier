namespace Entities.plant;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AirHumidity
{
    [Key] public int Id {get; set;}
    
    //EFC
    public int PlantId { get; set; }
    
    [ForeignKey(nameof(PlantId))]
    public Plant Plant { get; set; } = null!; //For EFC
}