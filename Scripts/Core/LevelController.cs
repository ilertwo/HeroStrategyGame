using Godot;
using System;

public partial class LevelController : Node
{
	[Export] public PackedScene GateScene;

	public void SpawnEvacuationGates()
	{
		if (GateScene == null)
		{
			GD.PrintErr("ПОМИЛКА: Не додано GateScene в Інспекторі!");
			return;
		}

		GD.Print("LevelController: Ворота з'являються!");

		Vector2[] spawnPositions = { new Vector2(500, 100), new Vector2(-500, 100), new Vector2(0, 500) };
		EvacuationGate.GateType[] types = { 
			EvacuationGate.GateType.MagicUpgrade, 
			EvacuationGate.GateType.EliteBoss, 
			EvacuationGate.GateType.Heal 
		};

		for (int i = 0; i < 3; i++)
		{
			EvacuationGate newGate = GateScene.Instantiate<EvacuationGate>();
			GetTree().CurrentScene.AddChild(newGate);
			newGate.GlobalPosition = spawnPositions[i];
			
			newGate.Setup(types[i]);
		}
	}
}
