using Godot;
using System;

public class Player : Node2D
{
	public Ship ship;
	public float health;
	public override void _Ready()
	{
		  
	}


	public static float runTime;

	public override void _Process(float delta)
	{
		runTime += delta;
	}
}
