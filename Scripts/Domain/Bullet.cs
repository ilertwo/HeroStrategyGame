using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public float Speed = 400.0f;
	[Export] public int Damage = 10;
	
	public Vector2 Direction = Vector2.Right; // Краще за замовчуванням дати напрямок вбік

	// Додаємо посилання на спрайт
	private Sprite2D _sprite;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		
		// Встановлюємо розмір один раз при появі
		_sprite.Scale = new Vector2(0.01f, 0.01f);

		BodyEntered += OnBodyEntered;
		
		var notifier = GetNodeOrNull<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		if (notifier != null)
		{
			notifier.ScreenExited += OnScreenExited;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += Direction * Speed * (float)delta;

		Rotation = Direction.Angle();
	}

	private void OnScreenExited()
	{
		QueueFree();
	}
	
	private void OnBodyEntered(Node2D body)
	{
		GD.Print($"Куля торкнулася об'єкта: {body.Name}"); 

		// Переконайтеся, що клас Enemy має таку ж назву і успадковується від Node2D/CharacterBody2D
		if (body is Enemy enemy)
		{
			GD.Print("Це ворог! Завдаємо шкоди.");
			enemy.TakeDamage(Damage);
			QueueFree();
		}
		else if (body is StaticBody2D) // Наприклад, якщо влучили в стіну
		{
			QueueFree();
		}
	}
}
