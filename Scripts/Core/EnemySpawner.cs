using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export] public Enemy PrototypeEnemy; 
	[ExportGroup("Межі спавну (Spawn Bounds)")]
	[Export] public float MinX = 50.0f;
	[Export] public float MaxX = 1100.0f; 
	[Export] public float MinY = 50.0f;
	[Export] public float MaxY = 600.0f;

	[ExportGroup("Налаштування часу")]
	[Export] public float SpawnInterval = 2.0f; 
	
	private double _timePassed = 0;

	public override void _Process(double delta)
	{
		if (PrototypeEnemy == null) return;

		_timePassed += delta;

		if (_timePassed >= SpawnInterval)
		{
			SpawnEnemyAtRandomPosition();
			_timePassed = 0;
		}
	}

	private void SpawnEnemyAtRandomPosition()
	{
		float randomX = (float)GD.RandRange(MinX, MaxX);
		float randomY = (float)GD.RandRange(MinY, MaxY);
		Vector2 randomPosition = new Vector2(randomX, randomY);

		Enemy newEnemy = PrototypeEnemy.Clone();
		
		newEnemy.Visible = true; 

		newEnemy.GlobalPosition = randomPosition;
		GetParent().AddChild(newEnemy);
		
		GD.Print($"Заспавнено клона на позиції: {randomPosition}");
	}
}
