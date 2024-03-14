using Godot;
using System;

public partial class enemy_base : CharacterBody2D
{

    // DECLARE STAT VALUES [Export] makes these numbers adjustable within the Godot engine
    [Export]
    public float speed = 30f;
    [Export]
    public int health = 10;

    // Declare a private variable to hold a reference to the player node
    private CharacterBody2D player;

    private Vector2 currentVelocity;

    // Method that is called when the scene is ready
    public override void _Ready()
    {
        // Access the scene tree and find the first node in the "player" group
        player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
    }

    // Physics Processing
    public override void _PhysicsProcess(double delta)
    {
        // Check if the player reference is not null
        if (player != null)
        {
            // Explicitly cast delta from double to float
            float floatDelta = (float)delta;
            currentVelocity = Vector2.Zero;

            // Calculate direction and distance to the player
            Vector2 direction = GlobalPosition.DirectionTo(player.GlobalPosition);
            float distance = GlobalPosition.DistanceTo(player.GlobalPosition);

            // Check if the enemy is close enough to the player
            float stopDistance = 0.5f; 
            if (distance > stopDistance)
            {
                // Setting velocity based on direction and speed
                currentVelocity = direction.Normalized() * speed;

                // Move the enemy towards the player using Translate
                Velocity = currentVelocity;
                MoveAndSlide();
            }
        }
    }

    private void OnHurtBoxHurt(int damage)
    {
        GD.Print("Enemy Hurtbox Entered!");
        health -= damage;
        GD.Print("Enemy Health:", health);
        if (health < 0)
        {
            QueueFree(); // Delete Enemy when destroyed
        }
    }   
}
