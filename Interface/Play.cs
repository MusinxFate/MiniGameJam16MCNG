using Godot;

public partial class Play : Node
{
	private void OnPlayButtonPressed()
	{
		{
			GetTree().ChangeSceneToFile("res://Interface/player.tscn");
		}
	}

}