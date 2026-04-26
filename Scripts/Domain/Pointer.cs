using Godot;
using System;

public partial class Pointer : Sprite2D
{
	public override void _Process(double delta)
	{
		var gates = GetTree().GetNodesInGroup("Gates");

		if (gates.Count == 0)
		{
			Visible = false;
			return;
		}

		Visible = true;
		Node2D targetGate = gates[0] as Node2D;

		if (targetGate != null)
		{
			GetParent<Node2D>().LookAt(targetGate.GlobalPosition);
		}
	}
}
