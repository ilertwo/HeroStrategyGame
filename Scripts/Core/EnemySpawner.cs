using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export] public Enemy PrototypeEnemy; 
	
	[ExportGroup("Налаштування спавну")]
	[Export] public float SpawnRadius = 1200.0f;
	[Export] public bool IsSquadMode = false;
	[Export] public int UnitsCount = 4;

	[ExportGroup("Налаштування часу")]
	[Export] public float SpawnInterval = 2.0f; 
	
	private double _timePassed = 0;
	private Node2D _player; 

	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("Player") as Node2D;

		if (PrototypeEnemy != null)
		{
			PrototypeEnemy.Visible = false;
			PrototypeEnemy.ProcessMode = Node.ProcessModeEnum.Disabled;
		}
	}

	public override void _Process(double delta)
	{
		if (PrototypeEnemy == null || _player == null) return;

		_timePassed += delta;

		if (_timePassed >= SpawnInterval)
		{
			SpawnEnemyAtRandomPosition();
			_timePassed = 0;
		}
	}

	private void SpawnEnemyAtRandomPosition()
	{
		float randomAngle = (float)GD.RandRange(0, Mathf.Pi * 2);
		
		Vector2 offset = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * SpawnRadius;
		Vector2 spawnPos = _player.GlobalPosition + offset;

		bool shouldSpawnSquad = IsSquadMode && GD.Randf() > 0.6f; 

		if (shouldSpawnSquad)
		{
			EnemySquad squad = new EnemySquad();
			for (int i = 0; i < UnitsCount; i++)
			{
				Enemy unit = PrototypeEnemy.Clone();
				unit.Visible = true;
				unit.ProcessMode = Node.ProcessModeEnum.Inherit;
				
				Vector2 squadOffset = new Vector2((float)GD.RandRange(-50, 50), (float)GD.RandRange(-50, 50));
				unit.GlobalPosition = spawnPos + squadOffset;
				
				unit.Modulate = new Color(1, 0.5f, 0.5f);
				unit.Scale = new Vector2(0.7f, 0.7f);
				
				GetParent().AddChild(unit);
				
				unit.OnDied += GameManager.Instance.AddScore;
				
				squad.AddUnit(unit);
			}
		}
		else
		{
			Enemy newEnemy = PrototypeEnemy.Clone();
			newEnemy.Visible = true;
			newEnemy.ProcessMode = Node.ProcessModeEnum.Inherit;
			
			newEnemy.GlobalPosition = spawnPos;
			
			GetParent().AddChild(newEnemy);
			
			newEnemy.OnDied += GameManager.Instance.AddScore;
		}
	}
}
