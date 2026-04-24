namespace Entities.plant;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LightIntensity
{
    [Key] public int Id {get; set;}

    //EFC
    public string PlantMAC { get; set; } = string.Empty;
    
    [ForeignKey(nameof(PlantMAC))]
    public Plant Plant { get; set; } = null!; //For EFC
}