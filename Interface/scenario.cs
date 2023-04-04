using Godot;
using System;

public partial class scenario : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //var player = (PackedScene)ResourceLoader.Load("res://player.tscn");
        //GetNode("/root/Scenario").AddChild(player.Instantiate<Player>());
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
