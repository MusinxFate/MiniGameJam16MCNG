using Godot;

public partial class Test : Button
{
    public string name = string.Empty;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Pressed += Test_Pressed;
    }

    private void Test_Pressed()
    {
        //var nameLineEdit = (Godot.LineEdit)GetNode("/root/Menu/Name");
        //var mpinfo = (Multiplayer)GetNode("/root/Multiplayer");
        //mpinfo.CharName = nameLineEdit.Text;
        //GetTree().ChangeSceneToFile("res://Interface/scenario.tscn");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
