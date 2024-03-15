using Godot;
using System;

// Enum to define different types of HurtBox
 public enum HurtBoxType
    {
        Cooldown,
        HitOnce,
        DisableHitBox
    }

public partial class hurtbox : Area2D
{
    // Exported property that can be adjusted in Godot Editor
   [Export] 
   HurtBoxType HurtBoxType { get; set; } = HurtBoxType.Cooldown;

    // Variables to hold references to CollisionShape2D and timer nodes 
   private CollisionShape2D collision;
   private Timer disableTimer;

   [Signal]
   public delegate void HurtEventHandler(int damage);

   private bool inContact = false;

   public override void _Ready()
   {
        // Obtain references to child nodes
        collision = GetNode<CollisionShape2D>("CollisionShape2D");
        disableTimer = GetNode<Timer>("disableTimer");

        // Connect the signal to appropriate method
        Connect("area_entered", new Godot.Callable(this, nameof(OnAreaEntered)));
        //Connect("area_exited", new Godot.Callable(this, nameof(OnAreaExited)));
        disableTimer.Connect("timeout", new Godot.Callable(this, nameof(OnDisableTimerTimeout)));
   }

    // Called when an object enters the area
    private void OnAreaEntered(Area2D area)
    {
        // Check if the entering area is in the "attack" group
        if (area.IsInGroup("attack"))
        {
            // Check if the entered area has a "damage" property
            if ((object)area.Get("damage") is not Godot.Variant.Type.Nil)
            {
                // Switch statement based on HurtBoxType enum
                switch (HurtBoxType)
                {
                    // If HurtBoxType is Cooldown (0)
                    case HurtBoxType.Cooldown:
                        collision.SetDeferred("disabled", true); // Disable collision shape
                        disableTimer.Start();    // State disableTimer
                        break; // Exit Switch Statement
                    case HurtBoxType.HitOnce:
                        // Pass
                        break;
                    case HurtBoxType.DisableHitBox:
                        if (area.HasMethod("tempdisable"))
                        {
                            area.Call("tempdisable");
                        }
                        break;
                }
                var damage = (int)area.Get("damage");
                EmitSignal("Hurt", damage);

            }
        }
    }

    private void OnDisableTimerTimeout()
    {
        collision.SetDeferred("disabled", false);
    }
}