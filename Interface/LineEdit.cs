using Godot;
using System;
using System.Linq;

public partial class LineEdit : Godot.LineEdit
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.TextChanged += LineEdit_TextChanged;
    }

    private void LineEdit_TextChanged(string newText)
    {
        foreach (var c in newText)
        {
            if (!Char.IsNumber(c) && Text.Contains(c) && c != '.')
            {
                Text = Text.Remove(Text.IndexOf(c), 1);
            }

            CaretColumn = Text.Length;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
