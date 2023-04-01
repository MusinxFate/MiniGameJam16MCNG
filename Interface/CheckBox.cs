using Godot;
using System;

public partial class CheckBox : Godot.CheckBox
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Toggled += CheckBox_Toggled;
    }

    private void CheckBox_Toggled(bool buttonPressed)
    {
        if (buttonPressed)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
            return;
        }

        DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
