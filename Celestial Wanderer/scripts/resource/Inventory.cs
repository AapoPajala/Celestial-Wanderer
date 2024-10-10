using Godot;
using System;
using System.Collections.Generic;

public class Inventory : Node
{
    public Dictionary<Resource.Resources, float> amounts;
    public static Dictionary<Resource.Resources, Resource> resourceInfos;

    public Inventory(int resNumber) {
        amounts = new Dictionary<Resource.Resources, float>();
    }
    public override void _Ready()
    {
        
    }

    public void transferTo(Inventory from, Resource.Resources res, float amount) {

        float a = from.amounts[res] > amount ? amount : from.amounts[res];

        if(!amounts.ContainsKey(res)) {
            amounts.Add(res, a);
        } else amounts[res] += a;

        from.amounts[res] -= a;
    }


}

public class ShipInventory : Inventory {

    float weightLimit;
    public ShipInventory(int resNumber, float limit) : base(resNumber) {
        weightLimit = limit;
    }

}
