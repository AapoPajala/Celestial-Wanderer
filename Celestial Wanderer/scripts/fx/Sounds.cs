using Godot;
using System;

public class Sounds : Node2D
{
    static Listener2D listener;
    public override void _Ready()
    {
        player = new AudioStreamPlayer2D();
        AddChild(player);
        listener = new Listener2D();
        AddChild(listener);
        listener.MakeCurrent();

        loadSounds();
    }
    public static AudioStreamSample combatStart, deathRay, enemyExplosion, engine1, friendlyContact;
    public static AudioStreamSample hostileAlert, laser, money, nukeExplosion, plasmaCannon;
    public static AudioStreamSample railgun, select, signalNoise;
    public void loadSounds() {
        combatStart = getRes("combat_start");
        deathRay = getRes("death_ray");
        enemyExplosion = getRes("enemy_explosion");
        engine1 = getRes("engine1");
        friendlyContact = getRes("friendly_contact");

        hostileAlert = getRes("hostile_alert");
        laser = getRes("laser");
        money = getRes("money");
        nukeExplosion = getRes("nuke_explosion");
        plasmaCannon = getRes("plasma_cannon");

        railgun = getRes("railgun");
        select = getRes("select");
        signalNoise = getRes("signal_noise");
    }

    AudioStreamSample getRes(String res) {
        return GD.Load<AudioStreamSample>("res://sounds/"+res+".wav");
    }

    static AudioStreamPlayer2D player;

    public static void playSound(AudioStream audio, Vector2 position) {
        
        player.GlobalPosition = position;
        player.Stream = audio;
        player.Play();
    }

    public override void _Process(float delta)
    {
        
    }
}
