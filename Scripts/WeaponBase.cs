using Godot;
using System;

public partial class WeaponBase : Node2D
{
    public float attackDamage = 10.0f;
    public float knockbackForce = 100.0f;
    public float stunTime = 1.5f;

    // Play the animation
    public override void _Ready()
    {
    var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
    animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
    }
}
