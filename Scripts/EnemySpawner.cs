using Godot;
using System;

public partial class EnemySpawner : Node2D
{
    // Reference to player node
    private CharacterBody2D player;
    // Counter to track time
    private int time  = 0;
    private Timer spawnTimer;


    public override void _Ready()
    {
        // Get Player Node
        player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
        spawnTimer = GetNode<Timer>("spawnTimer");
        
        // Timer
        spawnTimer.Connect("timeout", new Godot.Callable(this, nameof(OnSpawnTimerTimeout)));

        spawnTimer.Start();
    }

    public void OnSpawnTimerTimeout()
    {
        // Increment time counter
        time++;
        GD.Print($"Current time: {time}");

         // Initialize spawns array (Change to JSON files to condense and preset level values.)
        var spawns = new SpawnInfo[]
        {
            new SpawnInfo
            {
                TimeStart = 0,
                TimeEnd = 7,
                Enemy = GD.Load<PackedScene>("res://Entities/enemy_base.tscn"),
                EnemyNum = 1,
                EnemySpawnDelay = 0
            },
            new SpawnInfo
            {
                TimeStart = 7,
                TimeEnd = 60,
                Enemy = GD.Load<PackedScene>("res://Entities/enemy_base.tscn"),
                EnemyNum = 20,
                EnemySpawnDelay = 0
            }
            // Add more SpawnInfo instances as needed
        };

        // Loop through spawn information
        foreach (var spawn in spawns)
        {
            // Check if current time is within specified spawn time range
            if (time >= spawn.TimeStart && time < spawn.TimeEnd)
            {
                // Check if enough time has passed since last spawn
                if (spawn.SpawnDelayCounter < spawn.EnemySpawnDelay)
                {
                    spawn.SpawnDelayCounter--;
                }
                else
                {
                    // Reset delay counter
                    spawn.SpawnDelayCounter = 0;

                    // Load the enemy scene
                    for (int i = 0; i < spawn.EnemyNum; i++)
                    {
                        var newEnemyScene = (PackedScene)GD.Load(spawn.Enemy.ResourcePath);
                        if (spawn.Enemy != null)
                        {
                            var newEnemy = (Node2D)newEnemyScene.Instantiate();
                            if (newEnemy == null)
                            {
                                GD.PrintErr("Failed to instance enemy scene.");
                            }
                            else
                            {
                                GD.Print("Enemy scene instanced successfully.");
                                if (newEnemy.GetParent() != null)
                                {
                                    GD.PrintErr("Enemy already has a parent.");
                                }
                                else
                                {
                                    Node2D enemySpawn = (Node2D)newEnemy;
                                    enemySpawn.GlobalPosition = GetRandomPosition();
                                    AddChild(enemySpawn);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                GD.Print("Time is not within spawn range");
            }
        }
    }
    

    private Vector2 GetRandomPosition()
    {
        // Get the size of the viewport rect
        var vpr = GetViewportRect().Size * MathUtils.RandfRange(1.1f, 1.4f);

        // Calculate the positions of the corners of the viewport
        var topLeft = new Vector2(player.GlobalPosition.X - vpr.X / 2, player.GlobalPosition.Y - vpr.Y / 2);
        var topRight = new Vector2(player.GlobalPosition.X + vpr.X / 2, player.GlobalPosition.Y - vpr.Y / 2);
        var bottomLeft = new Vector2(player.GlobalPosition.X - vpr.X / 2, player.GlobalPosition.Y + vpr.Y / 2);
        var bottomRight = new Vector2(player.GlobalPosition.X + vpr.X / 2, player.GlobalPosition.Y + vpr.Y / 2);

        // Choose where to spawn
        string[] posSide = { "up", "down", "right", "left" };
        string spawnPosSide = posSide[new Random().Next(posSide.Length)];
        Vector2 spawnPos1 = Vector2.Zero;
        Vector2 spawnPos2 = Vector2.Zero;


        // Determine the spawn positions 
        switch (spawnPosSide)
        {
            case "up":
                spawnPos1 = topLeft;
                spawnPos2 = topRight;
                break;
            case "down":
                spawnPos1 = bottomLeft;
                spawnPos2 = bottomRight;
                break;
            case "right":
                spawnPos1 = topRight;
                spawnPos2 = bottomRight;
                break;
            case "left":
                spawnPos1 = topLeft;
                spawnPos2 = bottomLeft;
                break;
        }

        // Generate random corrdinates
        var xSpawn = MathUtils.RandfRange(spawnPos1.X, spawnPos2.X);
        var ySpawn = MathUtils.RandfRange(spawnPos1.Y, spawnPos2.Y);
        // Return random position
        return new Vector2(xSpawn, ySpawn);
    }
}