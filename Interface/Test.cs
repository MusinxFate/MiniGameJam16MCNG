using Godot;

public partial class Test : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.Pressed += Test_Pressed;
	}

    private void Test_Pressed()
    {
        GetTree().ChangeSceneToFile("res://Interface/player.tscn");
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
