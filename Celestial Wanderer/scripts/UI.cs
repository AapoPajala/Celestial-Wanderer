using Godot;
using System;

public class UI : Control
{
    Vector2 screen;
    Camera2D camera;
    public override void _Ready()
    {
        screen = GetViewport().Size;
        camera = GetParent() as Camera2D;

        friendly = new Sprite();
        friendly.Texture = GD.Load<Texture>("res://textures/icons/civ/friendly.png");
        AddChild(friendly);
        friendly.Position = new Vector2(256, -320);
        friendly.Visible = false;

        hostile = new Sprite();
        hostile.Texture = GD.Load<Texture>("res://textures/icons/civ/hostile.png");
        AddChild(hostile);
        hostile.Position = new Vector2(256, -320);
        hostile.Visible = false;

        intelligent = new Sprite();
        intelligent.Texture = GD.Load<Texture>("res://textures/icons/civ/intelligent.png");
        AddChild(intelligent);
        intelligent.Position = new Vector2(256, -384);
        intelligent.Visible = false;


        civilization = new Sprite();
        civilization.Texture = GD.Load<Texture>("res://textures/icons/civ/civ.png");
        AddChild(civilization);
        civilization.Position = new Vector2(256, -448);
        civilization.Visible = false;


    }

    public Sprite friendly, hostile, intelligent, civilization;

    public void showCiv(Civilization civ, bool show) {
        civilization.Visible = show;
        if(civ.intelligent) intelligent.Visible = show;
        if(civ.status == Civilization.Status.FRIENDLY) {
            friendly.Visible = show;
        }
        if(civ.status == Civilization.Status.HOSTILE) {
            hostile.Visible = show;
        }
    }

    public void showTerminal(bool show) {
        if(show) {

        } else {

        }
    }

    public override void _Process(float delta)
    {
        RectScale = camera.Zoom;
    }
}
