using Godot;
using System;

public partial class JoinGame : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Pressed += JoinGame_Pressed;
    }

    private void JoinGame_Pressed()
    {
        var x = (Multiplayer)GetNode("/root/Multiplayer");
        x.JoinServer();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
