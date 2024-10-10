using Godot;
using System;

public class Resource : InventoryItem
{
    public float basePrice;
    public Resources name;

    public enum Resources {
        COAL, NATGAS, OIL,              //Hydrocarbons
        URANIUM, THORIUM, HELIUM3,      //Nuclear
        DEUTERIUM, TRITIUM,
        IRON, COPPER, LEAD, ALUMINIUM,  //Metals
        TITANIUM, NICKEL, GOLD, SILVER,
        PLATINUM, ZINC,
        STONE, GEMSTONES, DIAMOND,      //Rocks (THEY'RE MINERALS, JESUS MARIE)
        SILICON,

    }

    public override void _Ready()
    {
        
    }

    public Resource getRandomResource(int amount) {
        Random r = new Random();
        Resources[] res = Enum.GetValues(typeof(Resources)) as Resources[];
        Resource resource = new Resource();
        resource.name = res[r.Next(res.Length - 1)];
        resource.amount = amount;

        return resource;
    }

}
