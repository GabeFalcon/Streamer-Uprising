using Godot;
using System;

public partial class asmongold : CharacterBody2D // Declares a class that inherits from the Godot node class
{
	[Export]
	public float speed = 300f;
	private AnimationTree animationTree; // Declares a private variable 'animationTree' of type 'AnimationTree'
	private Vector2 currentVelocity;
	private Sprite2D sprite;

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		handleInput();

		Velocity = currentVelocity;

		MoveAndSlide();
	}

	public override void _Process(double delta)
		{
			UpdateAnimatedParameters();
			UpdateFacingDirection();
		}

	public override void _Ready()	// Call when node and children are added to scene
	{
		animationTree = GetNode<AnimationTree>("AnimationTree"); // Access the 'AnimationTree' node in the scene and assigns it to 'animationTree'
		animationTree.Active = true; // Activate Animation Tree
		sprite = GetNode<Sprite2D>("AsmongoldSpritesheet");
	}

	private void UpdateAnimatedParameters()
	{
			animationTree.Set("parameters/Move/blend_position", currentVelocity.X);
	}

	private void UpdateFacingDirection()
	{
		if (currentVelocity.X < 0)
		{
			sprite.FlipH = false;
		}
		else if (currentVelocity.X > 0)
		{
			sprite.FlipH = true;
		}
	}

	private void handleInput()
    {
        currentVelocity = Vector2.Zero;

        if (Input.IsActionPressed("left"))
            currentVelocity.X -= 1;
        if (Input.IsActionPressed("right"))
            currentVelocity.X += 1;
        if (Input.IsActionPressed("up"))
            currentVelocity.Y -= 1;
        if (Input.IsActionPressed("down"))
            currentVelocity.Y += 1;

        currentVelocity = currentVelocity.Normalized() * speed;
    }
}


