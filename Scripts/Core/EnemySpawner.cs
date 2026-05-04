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

	private EnemySquad _recruitingSquad = new EnemySquad();
	private int _recruitedCount = 0; 
	private Vector2 _rallyPoint;

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

		if (GD.Randf() <= 0.3f)
		{
			if (_recruitedCount == 0)
			{
				float rallyX = (float)GD.RandRange(MinX, MaxX);
				float rallyY = (float)GD.RandRange(MinY, MaxY);
				_rallyPoint = new Vector2(rallyX, rallyY);
				GD.Print($"[Командир] Визначено нову точку збору: {_rallyPoint}");
			}

			_recruitingSquad.AddUnit(newEnemy);
			_recruitedCount++;
			
			newEnemy.MoveTowards(_rallyPoint);
			GD.Print($"[Вербування] Ворог біжить на точку збору! В загоні: {_recruitedCount}/4");

			if (_recruitedCount >= 4)
			{
				GD.Print(">>> КОМАНДИР: Загін зібрано! СПІЛЬНА АТАКА!");
				
				Node2D player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
				if (player != null)
				{
					_recruitingSquad.MoveTowards(player.GlobalPosition);
				}

				_recruitingSquad = new EnemySquad();
				_recruitedCount = 0;
			}
		}
		else
		{
			GD.Print($"[Одиночка] Заспавнено на {randomPosition}. Атакує сам.");
		}
	}
}
