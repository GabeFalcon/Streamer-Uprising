using Godot;
using System;

public partial class hitbox : Area2D
{
    // Declare damage value and export
    [Export]
    public int damage = 1;

    private CollisionShape2D collision;
    private Timer disableTimer;
    
    public override void _Ready()
    {
        collision = GetNode<CollisionShape2D>("CollisionShape2D");
        disableTimer = GetNode<Timer>("DisableHitboxTimer");
    }
    public void TempDisable()
    {
        collision.SetDeferred("disabled", true);
        disableTimer.Start();
    }
    
    private void OnDisableHitBoxTimerTimeout()
    {
        collision.SetDeferred("disabled", false);
    }
}
