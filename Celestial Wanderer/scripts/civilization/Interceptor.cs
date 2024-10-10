using Godot;
using System;

public class Interceptor : Node2D
{
    public Sprite sprite;
    public ShipWeapon weapon;
    public float health, fireRate;
    Player player;
    float timer;

    public override void _Ready()
    {
        Name = "Interceptor";
        player = GetParent().GetNode("Player") as Player;
        AddChild(weapon);
        AddChild(sprite);
    }


    public override void _Process(float delta)
    {
        if(timer > 0) timer -= delta;
        else {
            weapon.fire();
            timer = fireRate;
        }
    }
}
