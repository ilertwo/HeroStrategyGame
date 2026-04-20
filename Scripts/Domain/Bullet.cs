using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public float Speed = 400.0f;
	[Export] public int Damage = 10;
	
	public Vector2 Direction = Vector2.Zero;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").ScreenExited += OnScreenExited;
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += Direction * Speed * (float)delta;
	}

	private void OnScreenExited()
	{
		QueueFree();
	}
	
	private void OnBodyEntered(Node2D body)
	{
		GD.Print($"Куля торкнулася об'єкта: {body.Name}"); 

		if (body is Enemy enemy)
		{
			GD.Print("Це ворог! Завдаємо шкоди.");
			enemy.TakeDamage(Damage);
			QueueFree(); // Знищуємо кулю
		}
	}
}
