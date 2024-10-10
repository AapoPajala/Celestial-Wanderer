using Godot;
using System;
using System.Collections.Generic;

public class Civilization : Node2D
{
    Planet[] planets;
    Moon[] moons;
    public float lastVisit;
    public int tier;
    Inventory inventory;
    public int population;
    Dictionary<Resource.Resources, int> consumption;
    Dictionary<Resource.Resources, int> harvesting;
    Dictionary<Resource.Resources, int> prices;
    public Status status;
    Star star;
    public bool intelligent;

    public override void _Ready()
    {
        Random r = new Random();
        star = GetParent() as Star;
        
        
    }

    public void load() {
        Random r = new Random();

        planets = new Planet[r.Next(star.planets.Length)];

        consumption = new Dictionary<Resource.Resources, int>();
        harvesting = new Dictionary<Resource.Resources, int>();

        for(int i = 0; i < planets.Length; i++) {
            planets[i] = star.planets[i];
        }


        Status[] st = Enum.GetValues(typeof(Status)) as Status[];
        status = st[r.Next(st.Length-1)];
        tier = r.Next(1, 3);

        foreach(Planet p in planets) {
            foreach(Resource.Resources res in p.inventory.amounts.Keys) {
                float amount = consumption[res];
                inventory.transferTo(p.inventory, res, amount);

            }
        }
    }

    public void rates() {
        if(tier == 1) {
            consumption.Add(Resource.Resources.COAL, 14000);

        }
        if(tier == 2) {
            consumption.Add(Resource.Resources.URANIUM, 14000);

        }
        if(tier == 3) {
            consumption.Add(Resource.Resources.DEUTERIUM, 14000);

        }
    }

    public enum Status {
        FRIENDLY, HOSTILE, NEUTRAL
    }

    public override void _Process(float delta) {

        lastVisit = Player.runTime;
    }

    public void update() {
        foreach(Resource.Resources res in inventory.amounts.Keys) {
            float consume = consumption[res] * (Player.runTime - lastVisit);
            inventory.amounts[res] -= consume;
            if(inventory.amounts[res] < 0) inventory.amounts[res] = 0;

            foreach(Satellite s in star.planets) {
                float harvest = harvesting[res] * (Player.runTime - lastVisit);
                inventory.transferTo(s.inventory, res, harvest);
                
                foreach(Moon m in (s as Planet).moons) {
                    float harvest2 = harvesting[res] * (Player.runTime - lastVisit);
                    inventory.transferTo(m.inventory, res, harvest2);
                }
            }
        }
    }
}
