using Godot;
using System;

public partial class characterbase : CharacterBody2D // Declares a class that inherits from the Godot node class
{

	// DECLARE STAT VALUES (Speed, Health, ETC.) - [Export] makes these numbers adjustable within the Godot engine
	[Export]
	public float speed = 70f; 
	[Export]
	public int health = 80; 

	private AnimationTree animationTree; // Declares a private variable 'animationTree' of type 'AnimationTree'
	private Vector2 currentVelocity;
	private Sprite2D sprite; 

	// Movement, Gravity, Collisions, Forces Control
	public override void _PhysicsProcess(double delta)
	{
		// Explicitly cast delta from double to float
    	float floatDelta = (float)delta;
		
		// Update Physics Simulation for time step delta
		base._PhysicsProcess(delta);

		handleInput(); 

		Velocity = currentVelocity; // Set te velocity of the character to the current velocity vector
		MoveAndSlide(); // Apply movement and collisions based on current velocity
	}

	// NonPhyiscs Process Ex: AnimationSpeed, Direction, Etc.
	public override void _Process(double delta)
		{
			// Explicitly cast delta from double to float
            float floatDelta = (float)delta;
			UpdateAnimatedParameters();
			UpdateFacingDirection();
		}

	// Setting up initial state and references to other nodes in the scene hierarchy, making sure everything is properly initialized before the node starts processing.
	public override void _Ready()
	{
		animationTree = GetNode<AnimationTree>("AnimationTree"); // Access the 'AnimationTree' node in the scene and assigns it to 'animationTree'
		animationTree.Active = true; // Activate Animation Tree
		sprite = GetNode<Sprite2D>("AsmongoldSpritesheet");

		// Access The Hurtbox Script
		hurtbox hurtboxInstance = GetNode<hurtbox>("Hurtbox");


		hurtboxInstance.Connect("Hurt", new Godot.Callable(this, nameof(OnHurtboxHurt)));
	}

	public void OnHurtboxHurt(int damage)
	{
		health -= damage;
		GD.Print("Health:", health);
	}

	// User Inputs
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

	// Parameters for Animations
	private void UpdateAnimatedParameters()
	{
		// Calculate the magnitude of the current velocity
		float velocityMagnitude = currentVelocity.Length(); // This is to play the animation of running when moving up and down as well

		// Set blend_poistion based on the magnitude of the velocity
		animationTree.Set("parameters/Move/blend_position", velocityMagnitude);
			
	}

	// Flip Sprite 
	private void UpdateFacingDirection()
	{
		if (currentVelocity.X < 0) // Face Right When Moving Right
		{
			sprite.FlipH = false;
		}
		else if (currentVelocity.X > 0) // Face Left When Moving Left
		{
			sprite.FlipH = true;
		}
	}
}