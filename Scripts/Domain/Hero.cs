using Godot;
using System;

public partial class Hero : CharacterBody2D
{
	[Export] public float Speed = 200.0f;
	
	[Signal] public delegate void HealthChangedEventHandler(int newHealth);

	private int _health = 100;
	public int Health 
	{
		get => _health;
		set 
		{
			_health = value;
			EmitSignal(SignalName.HealthChanged, _health);
		}
	}

	private IFactoryWeapon _equippedWeapon; 
	private IArmor _equippedArmor;          
	private IWeapon _shootingLogic;

	// Патерн State
	private enum HeroState { Idle, Moving, Shooting }
	private HeroState _currentState = HeroState.Idle;

	public override void _Ready()
	{
		AddToGroup("Player");

		IEquipmentFactory developmentBranch = new MagicDevelopmentFactory(); 
		
		_equippedWeapon = developmentBranch.CreateWeapon();
		_equippedArmor = developmentBranch.CreateArmor();

		_equippedWeapon.Attack();
		_equippedArmor.Protect();

		_shootingLogic = new BasicGun();
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleMovement();
		HandleShooting();
	}

	private void HandleMovement()
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		
		Velocity = inputDir * Speed;
		MoveAndSlide();

		if (inputDir != Vector2.Zero) 
			_currentState = HeroState.Moving;
		else if (_currentState != HeroState.Shooting) 
			_currentState = HeroState.Idle;
	}

	private void HandleShooting()
	{
		Vector2 shootDir = Vector2.Zero;
		if (Input.IsActionJustPressed("shoot_up")) shootDir = Vector2.Up;
		if (Input.IsActionJustPressed("shoot_down")) shootDir = Vector2.Down;
		if (Input.IsActionJustPressed("shoot_left")) shootDir = Vector2.Left;
		if (Input.IsActionJustPressed("shoot_right")) shootDir = Vector2.Right;

		if (shootDir != Vector2.Zero)
		{
			_currentState = HeroState.Shooting;
			
			_shootingLogic.Shoot(this, shootDir);

			if (HasNode("AnimationPlayer"))
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("shoot");
			}
		}
	}

	public void UpgradeWeapon()
	{
		_shootingLogic = new FireBulletDecorator(_shootingLogic);
		GD.Print("Зброю покращено: тепер кулі вогняні!");
	}
}
