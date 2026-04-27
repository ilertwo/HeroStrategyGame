using Godot;
using System.Collections.Generic;

public class EnemySquad : IEnemyUnit
{
	private List<IEnemyUnit> _units = new List<IEnemyUnit>();

	public void AddUnit(IEnemyUnit unit)
	{
		if (!_units.Contains(unit)) _units.Add(unit);
	}

	public void RemoveUnit(IEnemyUnit unit)
	{
		_units.Remove(unit);
	}

	public void MoveTowards(Vector2 targetPosition)
	{
		GD.Print("--- ЗАГІН: Усім змінити позицію! ---");
		foreach (var unit in _units)
		{
			if (GodotObject.IsInstanceValid(unit as Node))
			{
				unit.MoveTowards(targetPosition);
			}
		}
	}

	public void TakeDamage(int amount)
	{
		GD.Print($"--- ЗАГІН: Масовий урон ({amount})! ---");
		foreach (var unit in _units)
		{
			if (GodotObject.IsInstanceValid(unit as Node))
			{
				unit.TakeDamage(amount);
			}
		}
	}
}
