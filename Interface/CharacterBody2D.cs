using Godot;
using System;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -400.0f;


    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private AnimationPlayer animPlayer; // Move a declaração do nó AnimationPlayer aqui fora.

    public override void _Ready()
    {
        var infoMultiplayer = (Multiplayer)GetNode("/root/Multiplayer");
        var nameLabel = (Label)GetNode("CharName");

        nameLabel.Text = infoMultiplayer.CharName;

        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("idle");
    }

    public override void _PhysicsProcess(double delta)
    {
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
    }
}
