using Godot;
using System;
using System.Diagnostics;

public class Star : Node2D
{

    public Sprite sprite;
    public Area2D area;
    public Planet[] planets;
    public Node2D children;
    public bool loaded;
    public override void _Ready()
    {
        SetProcess(false);

        sprite = new Sprite();
        sprite.Texture = GD.Load<Texture>("res://textures/celestial/star.png");
        AddChild(sprite);
        Scale = new Vector2(0.25f, 0.25f);
        area = new Area2D();
        CircleShape2D circle = new CircleShape2D();
        CollisionShape2D shape = new CollisionShape2D();
        shape.Shape = circle;
        circle.Radius = sprite.Texture.GetSize().x;
        area.InputPickable = true;
        area.CollisionLayer = 1;
        AddChild(area);
        area.AddChild(shape);

        area.Connect("mouse_entered", this, nameof(onMouseEntered));
        area.Connect("mouse_exited", this, nameof(onMouseExited));

        children = new Node2D();
        AddChild(children);
        children.Visible = false;

        Random r = new Random();
        int c = r.Next(3);
        Color color = Colors.White;
        if(c == 0) color = Colors.White;
        //if(c == 1) color = Colors.Red;
        if(c == 2) color = Colors.LightBlue;
        if(c == 3) color = Colors.Yellow;

        color.a = 0.85f;
        sprite.Modulate = color;

        if(r.Next(20) == 11) {
            civilization = new Civilization();
            AddChild(civilization);
        }
    }

    public Civilization civilization;

    public void suspended(bool paused) {
        SetProcess(!paused);

        foreach(Node2D n in children.GetChildren()) {
            n.SetProcess(!paused);
            foreach(Node2D m in n.GetChildren())
                m.SetProcess(!paused);
        }
    }

    public void load() {
        Random r = new Random();
        planets = new Planet[r.Next(1, 4)];

        for(int i = 0; i < planets.Length; i++) {
            Planet s = planets[i] = new Planet();
            s.radius = Math.Max(r.Next(64 / (i + 1)), 16);
            children.AddChild(s);
            s.Position = new Vector2(128*(i + 1), 0);
        }
        loaded = true;
    }

    public void onMouseEntered() {
        if(selectedStar == null) {
            selectedStar = this;
            if(civilization != null) {
                Sounds.playSound(Sounds.signalNoise, GlobalPosition);
            }
        }
        Update();
    }

    public static Star selectedStar;

    public void onMouseExited() {
        selectedStar = null;
        Update();
    }

    static Color color = new Color(0.6f, 0.6f, 0.9f, 0.3f);

    public override void _Draw() {

        if(Equals(selectedStar) && Travel.view == Travel.NavView.INTERSTELLAR) 
            DrawCircle(Vector2.Zero, sprite.Texture.GetSize().x, color);
        base._Draw();
    }

}
