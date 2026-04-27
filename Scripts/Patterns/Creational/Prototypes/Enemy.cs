using Godot;
using System;

public interface IPrototype<T>
{
	T Clone();
}

public partial class Enemy : CharacterBody2D, IPrototype<Enemy>, IEnemyUnit
{
	[Export] public EnemyStats Stats; 

	private int _currentHealth;
	
	private Node2D _target;
	private Vector2? _squadTarget = null;

	public override void _Ready()
	{
		_target = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		
		if (Stats != null)
		{
			_currentHealth = Stats.MaxHealth;
		}
		else
		{
			GD.PrintErr("У ворога не задано ресурс EnemyStats!");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Stats == null) return;

		Vector2 destination = _squadTarget.HasValue ? _squadTarget.Value : (_target != null ? _target.GlobalPosition : GlobalPosition);

		if (GlobalPosition.DistanceTo(destination) > 10.0f)
		{
			Vector2 direction = GlobalPosition.DirectionTo(destination);
			
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
			GameManager.Instance.AddScore(10);
			QueueFree(); 
		}
	}

	public void MoveTowards(Vector2 targetPosition)
	{
		GD.Print($"Клон перемикає ціль! Біжу на {targetPosition}");
		_squadTarget = targetPosition;
	}

	public Enemy Clone()
	{
		return (Enemy)this.Duplicate(); 
	}
}
