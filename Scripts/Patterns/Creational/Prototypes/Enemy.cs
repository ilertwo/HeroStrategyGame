using Godot;
using System;

public interface IPrototype<T>
{
	T Clone();
}

public partial class Enemy : CharacterBody2D, IPrototype<Enemy>
{
	[Export] public float Speed = 200.0f;
	[Export] public int Health = 30;

	private Node2D _target;

	public override void _Ready()
	{
		_target = GetTree().GetFirstNodeInGroup("Player") as Node2D;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_target != null)
		{
			Vector2 direction = GlobalPosition.DirectionTo(_target.GlobalPosition);
			Velocity = direction * Speed;
			MoveAndSlide();
		}
	}

	public void TakeDamage(int damage)
	{
		if (!Visible) return;

		Health -= damage;
		GD.Print($"Ворог отримав {damage} шкоди. Залишилось ХП: {Health}");

		if (Health <= 0)
		{
			GameManager.Instance.AddScore(10);
			QueueFree(); 
		}
	}

	// Реалізація патерну Прототип
	public Enemy Clone()
	{
		return (Enemy)this.Duplicate(); 
	}
}
