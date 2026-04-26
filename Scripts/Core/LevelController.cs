using Godot;
using System;

public partial class LevelController : Node
{
	[Export] public PackedScene GateScene; // Закинь сюди EvacuationGate.tscn в редакторі
	private bool _gatesSpawned = false;
	private int _targetScore = 50;

	public override void _Process(double delta)
	{
		if (!_gatesSpawned && GameManager.Instance.Score >= _targetScore)
		{
			SpawnEvacuationGates();
			_gatesSpawned = true;
		}
	}

	private void SpawnEvacuationGates()
	{
		GD.Print("Рахунок досягнуто! Ворота з'являються!");


		Vector2[] spawnPositions = { new Vector2(500, 100), new Vector2(-500, 100), new Vector2(0, 500) };
		EvacuationGate.GateType[] types = { 
			EvacuationGate.GateType.MagicUpgrade, 
			EvacuationGate.GateType.EliteBoss, 
			EvacuationGate.GateType.Heal 
		};

		for (int i = 0; i < 3; i++)
		{
			EvacuationGate newGate = GateScene.Instantiate<EvacuationGate>();
			GetParent().AddChild(newGate);
			newGate.GlobalPosition = spawnPositions[i];
			
			newGate.Setup(types[i]);
		}
	}
}
