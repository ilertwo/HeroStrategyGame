using Godot;
using System;

public interface IWeapon
{
	void Shoot(Node2D startPoint, Vector2 direction);
}

public class BasicGun : IWeapon
{
	private PackedScene _bulletScene;

	public BasicGun()
	{
		_bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
	}

	public void Shoot(Node2D shooter, Vector2 direction)
	{
		if (_bulletScene == null)
		{
			GD.Print("ПОМИЛКА: Сцена кулі не завантажена. Перевір шлях у конструкторі!");
			return;
		}

		Bullet bullet = _bulletScene.Instantiate<Bullet>();

		bullet.Direction = direction;
		bullet.GlobalPosition = shooter.GlobalPosition;

		shooter.GetParent().AddChild(bullet);
		
	}
}

public abstract class WeaponDecorator : IWeapon
{
	protected IWeapon _wrappedWeapon;

	public WeaponDecorator(IWeapon weapon)
	{
		_wrappedWeapon = weapon;
	}

	public virtual void Shoot(Node2D startPoint, Vector2 direction)
	{
		_wrappedWeapon.Shoot(startPoint, direction);
	}
}

public class FireBulletDecorator : WeaponDecorator
{
	public FireBulletDecorator(IWeapon weapon) : base(weapon) { }

	public override void Shoot(Node2D startPoint, Vector2 direction)
	{
		base.Shoot(startPoint, direction);
		GD.Print("ЕФЕКТ: Куля підпалює ворога!");
	}
}
