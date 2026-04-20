using Godot;
using System;

public partial class Hero : CharacterBody2D
{
	[Export] public float Speed = 200.0f;
	
	// Патерн Observer: Подія, яку буде слухати UI
	[Signal] public delegate void HealthChangedEventHandler(int newHealth);

	private int _health = 100;
	public int Health 
	{
		get => _health;
		set 
		{
			_health = value;
			EmitSignal(SignalName.HealthChanged, _health); // Сповіщаємо підписників
		}
	}

	private IWeapon _currentWeapon;

	// Патерн State (дуже спрощена версія для керування станами анімації/логіки)
	private enum HeroState { Idle, Moving, Shooting }
	private HeroState _currentState = HeroState.Idle;

	public override void _Ready()
	{
		AddToGroup("Player");
		_currentWeapon = new BasicTear(); // Стартова зброя
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleMovement();
		HandleShooting();
	}

	private void HandleMovement()
	{
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDir * Speed;
		MoveAndSlide();

		// Зміна стану
		_currentState = inputDir != Vector2.Zero ? HeroState.Moving : HeroState.Idle;
	}

	private void HandleShooting()
	{
		// Стрільба стрілочками (як в Isaac)
		Vector2 shootDir = Vector2.Zero;
		if (Input.IsActionJustPressed("shoot_up")) shootDir = Vector2.Up;
		if (Input.IsActionJustPressed("shoot_down")) shootDir = Vector2.Down;
		if (Input.IsActionJustPressed("shoot_left")) shootDir = Vector2.Left;
		if (Input.IsActionJustPressed("shoot_right")) shootDir = Vector2.Right;

		if (shootDir != Vector2.Zero)
		{
			_currentState = HeroState.Shooting;
			_currentWeapon.Shoot(this, shootDir);
		}
	}

	// Виклик цієї функції для тестування Декоратора (наприклад, при піднятті предмета)
	public void UpgradeWeapon()
	{
		_currentWeapon = new FireTearDecorator(_currentWeapon);
		GD.Print("Зброю покращено!");
	}
}
