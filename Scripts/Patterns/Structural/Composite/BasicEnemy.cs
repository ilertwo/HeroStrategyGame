using Godot;

public partial class BasicEnemy : CharacterBody2D, IEnemyUnit
{
	private int _health = 100;
	private string _name;

	public BasicEnemy(string name)
	{
		_name = name;
	}

	public void MoveTowards(Vector2 targetPosition)
	{
		// Тут була б логіка Godot (Velocity = ...; MoveAndSlide();)
		GD.Print($"[{_name}] Біжить до героя на {targetPosition}");
	}

	public void TakeDamage(int amount)
	{
		_health -= amount;
		GD.Print($"[{_name}] Отримав {amount} урону. Залишилось ХП: {_health}");
	}
}
