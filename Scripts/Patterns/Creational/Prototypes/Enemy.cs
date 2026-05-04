using Godot;
using System;

public interface IMovementStrategy
{
	Vector2 CalculateDirection(Vector2 currentPos, Vector2 targetPos);
}

public class ChaseStrategy : IMovementStrategy
{
	public Vector2 CalculateDirection(Vector2 currentPos, Vector2 targetPos)
	{
		return currentPos.DirectionTo(targetPos);
	}
}

public interface IPrototype<T>
{
	T Clone();
}

public partial class Enemy : CharacterBody2D, IPrototype<Enemy>, IEnemyUnit
{
	[Export] public EnemyStats Stats; 
	
	// === ПАТЕРН СПОСТЕРІГАЧ (Observer) ===
	// Ворог просто "кричить" про свою смерть, а хто слухає - його не хвилює
	public event Action<int> OnDied;

	private int _currentHealth;
	private Node2D _target;
	private Vector2? _squadTarget = null;
	
	// Поточна стратегія руху
	private IMovementStrategy _moveStrategy = new ChaseStrategy();

	public override void _Ready()
	{
		_target = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		
		if (Stats != null) _currentHealth = Stats.MaxHealth;
		else GD.PrintErr("У ворога не задано ресурс EnemyStats!");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Stats == null) return;
		ExecuteAITemplate(delta); // Виклик шаблонного методу
	}

	// === ПАТЕРН ШАБЛОННИЙ МЕТОД (Template Method) ===
	// Жорсткий скелет дій, який не можна ламати, але можна перевизначати кроки
	private void ExecuteAITemplate(double delta)
	{
		Vector2 destination = DetermineTarget();
		PerformMovement(destination);
	}

	protected virtual Vector2 DetermineTarget()
	{
		return _squadTarget.HasValue ? _squadTarget.Value : (_target != null ? _target.GlobalPosition : GlobalPosition);
	}

	protected virtual void PerformMovement(Vector2 destination)
	{
		if (GlobalPosition.DistanceTo(destination) > 10.0f)
		{
			// Використовуємо патерн Стратегія для обчислення напрямку
			Vector2 direction = _moveStrategy.CalculateDirection(GlobalPosition, destination);
			Velocity = direction * Stats.Speed; 
			LookAt(destination);
			MoveAndSlide();
		}
	}

	public void TakeDamage(int damage)
	{
		if (!Visible) return;

		_currentHealth -= damage;
		GD.Print($"Ворог отримав {damage} шкоди. Залишилось ХП: {_currentHealth}");

		if (_currentHealth <= 0)
		{
			// Замість GameManager.Instance.AddScore(10) смикаємо подію
			OnDied?.Invoke(10); 
			QueueFree(); 
		}
	}

	public void MoveTowards(Vector2 targetPosition)
	{
		_squadTarget = targetPosition;
	}

	public Enemy Clone()
	{
		return (Enemy)this.Duplicate(); 
	}
}
