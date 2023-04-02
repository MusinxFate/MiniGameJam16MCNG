using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;
    [Export]
    Vector2 syncPos = new Vector2();
    Inputs inputs = new Inputs();



    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimationPlayer animPlayer; // Move a declaração do nó AnimationPlayer aqui fora.

    public override void _Ready()
    {
        Inputs inps = (Inputs)GetNode("Inputs");
        Position = syncPos;
        if (StringExtensions.IsValidInt(Name))
            GetNode("Inptus/InputsSync").SetMultiplayerAuthority(StringExtensions.ToInt(Name));

        var infoMultiplayer = (Multiplayer)GetNode("/root/Multiplayer");
        var nameLabel = (Label)GetNode("CharName");

        nameLabel.Text = infoMultiplayer.CharName;

        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("idle");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Multiplayer.MultiplayerPeer == null || Multiplayer.MultiplayerPeer.GetUniqueId().ToString() == Name)
            inputs.Update();

        if (Multiplayer.MultiplayerPeer == null || IsMultiplayerAuthority())
        {
            syncPos = Position;
        }
        else
        {
            Position = syncPos;
        }

        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y += gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();

        syncPos = Position;
    }

    public void SetPlayerName(string name)
    {
        var x = (Label)GetNode("CharName");
        x.Text = name;
    }
}
