using Godot;
using System;

public class ShipWeapon : InventoryItem
{
    public float fireRate;
    public Projectile projectile;

    public override void _Ready()
    {
        
    }
    public override void _Process(float delta)
    {
        
    }

    public void fire() {
        GetTree().CurrentScene.AddChild(projectile);
    }
}
