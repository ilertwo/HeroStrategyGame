using Godot;
using System.Collections.Generic;

public class EnemySquad : IEnemyUnit
{
	private List<IEnemyUnit> _units = new List<IEnemyUnit>();

	public void AddUnit(IEnemyUnit unit) => _units.Add(unit);

	public void MoveTowards(Vector2 targetPosition)
	{
		// Всі юніти в списку отримують наказ одночасно
		foreach (var unit in _units) 
			unit.MoveTowards(targetPosition);
	}

	public void TakeDamage(int damage)
	{
		// Логіка: шкода дістається першому в черзі (або розподіляється)
		if (_units.Count > 0) _units[0].TakeDamage(damage);
	}
}
