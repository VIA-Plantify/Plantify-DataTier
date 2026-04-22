namespace Entities.plant;

public class SoilHumidity
{
    public int Id {get; set;}

    //EFC
    public int PlantId { get; set; }
    
    [ForeignKey(nameof(PlantId))]
    public Plant Plant { get; set; } = null!; //For EFC
}