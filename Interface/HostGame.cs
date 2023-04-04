using Godot;
using System;

public partial class HostGame : Button
{
    public override void _Ready()
    {
        this.Pressed += HostGame_Pressed;
    }

    public void HostGame_Pressed()
    {
        var x = (Multiplayer)GetNode("/root/Multiplayer");
        x.CreateServer();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
