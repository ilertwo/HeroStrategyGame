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
	[Export] public bool IsSquadMode = false;
	[Export] public int UnitsCount = 4;

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
		Vector2 centerPos = new Vector2((float)GD.RandRange(MinX, MaxX), (float)GD.RandRange(MinY, MaxY));

		bool shouldSpawnSquad = IsSquadMode && GD.Randf() > 0.6f; 

		if (shouldSpawnSquad)
		{
			EnemySquad squad = new EnemySquad();
			for (int i = 0; i < UnitsCount; i++)
			{
				Enemy unit = PrototypeEnemy.Clone();
				unit.Visible = true;
				Vector2 offset = new Vector2((float)GD.RandRange(-40, 40), (float)GD.RandRange(-40, 40));
				unit.GlobalPosition = centerPos + offset;
				unit.Modulate = new Color(1, 0.5f, 0.5f);
				unit.Scale = new Vector2(0.7f, 0.7f);
				GetParent().AddChild(unit);
				
				// +++ Підписуємо GameManager на смерть юніта із загону +++
				unit.OnDied += GameManager.Instance.AddScore;
				
				squad.AddUnit(unit);
			}
			GD.Print($"Група заспавнена у позиції {centerPos}");
		}
		else
		{
			Enemy newEnemy = PrototypeEnemy.Clone();
			newEnemy.Visible = true;
			newEnemy.GlobalPosition = centerPos;
			GetParent().AddChild(newEnemy);
			
			// +++ Підписуємо GameManager на смерть одиночки +++
			newEnemy.OnDied += GameManager.Instance.AddScore;
			
			GD.Print($"Одиночний ворог заспавнений у позиції {centerPos}");
		}
	}
}
