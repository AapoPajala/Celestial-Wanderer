using Godot;
using System;

public class Projectile : Node2D
{
    CollisionShape2D coll;
    RectangleShape2D rect;
    Sprite sprite;
    public float velocity;
    public float damage;
    public bool explosive, piercing;
    public float radius;
    public int dir;

    public override void _Ready()
    {
        coll = new CollisionShape2D();
        rect = new RectangleShape2D();
        rect.Extents = sprite.Texture.GetSize();
        coll.Shape = rect;
        AddChild(coll);
        coll.Connect("body_entered", this, nameof(hit));
    }


    public override void _Process(float delta)
    {
        Vector2 direction = new Vector2(0, dir);
        Translate(direction);
    }

    public void hit(Godot.Object body) {
        Node2D n = body as Node2D;
        if(n != null) {

            if(n.Name == "Player")
                (n as Player).health -= damage;

            if(n.Name == "Interceptor")
                (n as Interceptor).health -= damage;

        }
    }
}
