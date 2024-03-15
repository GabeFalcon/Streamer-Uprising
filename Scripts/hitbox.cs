using Godot;
using System;

public partial class hitbox : Area2D
{
    // Declare damage value and export
    [Export]
    public int damage = 1;

    private CollisionShape2D collision;
    private Timer disableTimer;

    [Signal]
    public delegate void HurtEventHandler(int damage);
    
    public override void _Ready()
    {
        collision = GetNode<CollisionShape2D>("CollisionShape2D");
        disableTimer = GetNode<Timer>("DisableHitboxTimer");

        // Connect the signals to appropriate methods
        disableTimer.Connect("timeout", new Godot.Callable(this, nameof(OnDisableHitboxTimerTimeout)));
    }
    public void tempdisable()
    {
        collision.SetDeferred("disabled", true);
        disableTimer.Start();
    }
    
    private void OnDisableHitboxTimerTimeout()
    {
        collision.SetDeferred("disabled", false);
    }
}
