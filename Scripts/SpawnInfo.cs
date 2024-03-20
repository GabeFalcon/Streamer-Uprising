using Godot;
using System;

public partial class SpawnInfo : Resource
{
    // Start time of the spawn
    [Export]
    public int TimeStart { get; set; }
    // End time of the spawn
    [Export]
    public int TimeEnd { get; set; }
    // Resource representing the enemy scene
    [Export]
    public Resource Enemy { get; set; }
    // Number of enemies to spawn
    [Export]
    public int EnemyNum { get; set; }
    // Delay between enemy spawns
    [Export]
    public int EnemySpawnDelay { get; set; }

    // Counter to track the delay between spawns
    public int SpawnDelayCounter = 0;
}

// Utility class for math operations
public static class MathUtils
{
    private static Random _random = new Random();

    // Generate a random float within the specified range
    public static float RandfRange(float min, float max)
    {
        return (float)(_random.NextDouble() * (max - min) + min);
    }
}

