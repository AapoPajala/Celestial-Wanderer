using Godot;
using System;
using System.Collections.Generic;

public class Ship : Node
{

    Inventory inventory;
    List<ShipWeapon> weapons;
    ShipWeapon equipped;
    public override void _Ready()
    {
        
    }

    public float timer;

    public override void _Process(float delta) {

        if(timer > 0) timer -= delta;
        else {
            equipped.fire();
            timer = equipped.fireRate;
        }

    }

    public void fire() {
    
    }
    
}