using Godot;
using System;

public partial class OptionButton : Godot.OptionButton
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ItemSelected += OptionButton_ItemSelected;
    }

    private void OptionButton_ItemSelected(long index)
    {
        var text = this.GetItemText(Selected);
        if (Selected != 0)
        {
            var resNotSplitted = GetItemText((int)Selected).Split("x");
            Vector2I res = new(int.Parse(resNotSplitted[0]), int.Parse(resNotSplitted[1]));
            DisplayServer.WindowSetSize(res);

            var screenSize = DisplayServer.ScreenGetSize();
            var windowSize = DisplayServer.WindowGetSize();

            var screenSizetest = ((int)Math.Round(screenSize.X * 0.5), (int)Math.Round(screenSize.Y * 0.5));
            var windowSizeTest = ((int)Math.Round(windowSize.X * 0.5), (int)Math.Round(windowSize.Y * 0.5));

            var pos = screenSizetest.Item1 - windowSizeTest.Item1;
            var pos2 = screenSizetest.Item2 - windowSizeTest.Item2;

            DisplayServer.WindowSetPosition(new Vector2I(pos, pos2), 0);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
