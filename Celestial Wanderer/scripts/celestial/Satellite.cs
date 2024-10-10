using Godot;
using System;

public class Satellite : Node2D
{
    Area2D area;
    public float radius;
    public bool loaded;
    public Inventory inventory;

    public Satellite() {
        
    }

    public override void _Ready()
    {
        area = new Area2D();
        CircleShape2D circle = new CircleShape2D();
        CollisionShape2D shape = new CollisionShape2D();
        shape.Shape = circle;
        circle.Radius = radius;
        area.InputPickable = true;
        area.CollisionLayer = 1;
        AddChild(area);
        area.AddChild(shape);

        area.Connect("mouse_entered", this, nameof(onMouseEntered));
        area.Connect("mouse_exited", this, nameof(onMouseExited));


    }

    public virtual void load() {
    
    }

    public override void _Process(float delta) {
       
    }

    public void onMouseEntered() {
        if(Travel.view != Travel.NavView.INTERSTELLAR) {
            selectedSatellite = this;
            Update();
        }
    }

    public static Satellite selectedSatellite;

    public void onMouseExited() {
        if(Travel.view != Travel.NavView.INTERSTELLAR) {
            selectedSatellite = null;
            Update();
        }
    }

    static Color color = new Color(0.6f, 0.6f, 0.9f, 0.3f);

    public override void _Draw() {

        if(Equals(selectedSatellite) && Travel.view != Travel.NavView.INTERSTELLAR)
            DrawCircle(Vector2.Zero, radius*2f, color);
        base._Draw();
    }

    public void orbitAround(Vector2 center) {
        Vector2 pos = new Vector2(GlobalPosition.x, GlobalPosition.y);
        
        float angle = 0;
        angle += 0.0017f / (GetIndex() + 1);

        float s = (float)Math.Sin(angle);
        float c = (float)Math.Cos(angle);

        pos.x -= center.x;
        pos.y -= center.y;

        float xnew = pos.x * c - pos.y * s;
        float ynew = pos.x * s + pos.y * c;

        pos.x = xnew + center.x;
        pos.y = ynew + center.y;

        GlobalPosition = pos;
    }
}

public class Planet : Satellite {

    public Moon[] moons;
    Color color;
    
    public Planet() {

        Random r = new Random();
        moons = new Moon[r.Next(4)];
        
        color = new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());

    }


    public override void load() {

        Random r = new Random();

        float next = 0;
        for(int i = 0; i < moons.Length; i++) {
            Moon s = moons[i] = new Moon();
            s.radius = r.Next((int)(radius / 2f));
            s.Visible = false;
            AddChild(s);
            s.Position += new Vector2(s.radius * 2 + 32 * (i + 1) + next, 0);
            next = s.radius * 4f;
            s.Translate(new Vector2(32, 0));
        }
        loaded = true;
    }

    public void showMoons(bool show) {
        for(int i = 0; i < moons.Length; i++) {
            Node2D n = moons[i];
            n.Visible = show;
        }
    }

    public override void _Process(float delta) {
        //GD.Print("reeeee");
        if(Travel.view == Travel.NavView.PLANETARY) {
            Vector2 star = (GetParent() as Node2D).GlobalPosition;
            orbitAround(star);
        }
        base._Process(delta);
    }

    public override void _Draw() {
        DrawCircle(Vector2.Zero, radius, color);
        base._Draw();
    }

}

public class Moon : Satellite {

    public Moon() {
       
    }

    public override void _Process(float delta) {
        if(Travel.view == Travel.NavView.ORBITAL) {
            Vector2 planet = (GetParent() as Node2D).GlobalPosition;
            orbitAround(planet);
        }
    }

    public override void _Draw() {
        DrawCircle(Vector2.Zero, radius, Colors.Gray);
        base._Draw();
    }

}

