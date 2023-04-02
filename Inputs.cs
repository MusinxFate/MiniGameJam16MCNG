using Godot;
using System;

public partial class Inputs : Node
{
    [Export]
    public Vector2 Motion = new Vector2();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Update()
    {
        var m = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Motion = m;
    }
}
