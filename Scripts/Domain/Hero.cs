using Godot;
using System;

// === ПАТЕРН КОМАНДА (Command) ===
public interface ICommand
{
	void Execute();
}

public class ShootCommand : ICommand
{
	private Hero _hero;
	private IWeapon _weapon;
	private Vector2 _direction;

	public ShootCommand(Hero hero, IWeapon weapon, Vector2 direction)
	{
		_hero = hero;
		_weapon = weapon;
		_direction = direction;
	}

	public void Execute()
	{
		_weapon.Shoot(_hero, _direction);
		if (_hero.HasNode("AnimationPlayer"))
		{
			_hero.GetNode<AnimationPlayer>("AnimationPlayer").Play("shoot");
		}
	}
}

// === ПАТЕРН СТАН (State) ===
public interface IHeroState
{
	IHeroState HandleInput(Hero hero);
	void UpdateState(Hero hero, double delta);
}

public class IdleState : IHeroState
{
	public IHeroState HandleInput(Hero hero)
	{
		if (Input.GetVector("move_left", "move_right", "move_up", "move_down") != Vector2.Zero)
			return new MovingState();
		return this;
	}
	public void UpdateState(Hero hero, double delta) { hero.Velocity = Vector2.Zero; hero.MoveAndSlide(); }
}

public class MovingState : IHeroState
{
	public IHeroState HandleInput(Hero hero)
	{
		if (Input.GetVector("move_left", "move_right", "move_up", "move_down") == Vector2.Zero)
			return new IdleState();
		return this;
	}
	public void UpdateState(Hero hero, double delta)
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		hero.Velocity = inputDir * hero.Speed;
		hero.MoveAndSlide();
	}
}


public partial class Hero : CharacterBody2D
{
	[Export] public float Speed = 200.0f;
	[Signal] public delegate void HealthChangedEventHandler(int newHealth);

	private int _health = 100;
	public int Health 
	{
		get => _health;
		set { _health = value; EmitSignal(SignalName.HealthChanged, _health); }
	}

	private IFactoryWeapon _equippedWeapon; 
	private IArmor _equippedArmor;          
	public IWeapon ShootingLogic; // Зроблено public для доступу з команди

	// Поточний стан
	private IHeroState _currentState = new IdleState();

	public override void _Ready()
	{
		AddToGroup("Player");
		IEquipmentFactory developmentBranch = new MagicDevelopmentFactory(); 
		_equippedWeapon = developmentBranch.CreateWeapon();
		_equippedArmor = developmentBranch.CreateArmor();
		ShootingLogic = new BasicGun();
	}

	public override void _PhysicsProcess(double delta)
	{
		// 1. Делегуємо рух поточному стану
		IHeroState nextState = _currentState.HandleInput(this);
		if (nextState != _currentState) _currentState = nextState;
		
		_currentState.UpdateState(this, delta);

		// 2. Обробляємо постріл через Команду
		HandleShooting();
	}

	private void HandleShooting()
	{
		Vector2 shootDir = Vector2.Zero;
		if (Input.IsActionJustPressed("shoot_up")) shootDir = Vector2.Up;
		else if (Input.IsActionJustPressed("shoot_down")) shootDir = Vector2.Down;
		else if (Input.IsActionJustPressed("shoot_left")) shootDir = Vector2.Left;
		else if (Input.IsActionJustPressed("shoot_right")) shootDir = Vector2.Right;

		if (shootDir != Vector2.Zero)
		{
			Rotation = shootDir.Angle() - Mathf.Pi / 2;
			
			// Використовуємо патерн Команда
			ICommand shootAction = new ShootCommand(this, ShootingLogic, shootDir);
			shootAction.Execute();
		}
	}

	public void UpgradeWeapon()
	{
		ShootingLogic = new FireBulletDecorator(ShootingLogic);
		GD.Print("Зброю покращено: тепер кулі вогняні!");
	}
}
