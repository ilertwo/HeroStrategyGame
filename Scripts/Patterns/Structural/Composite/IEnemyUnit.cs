using Godot;

public interface IEnemyUnit
{
	void MoveTowards(Vector2 targetPosition);
	void TakeDamage(int amount);
}
