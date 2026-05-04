using Godot;
using System;

public interface IWeapon
{
	Node2D Shoot(Node2D startPoint, Vector2 direction);
}

public class BaseGun : IWeapon
{
	private PackedScene _bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
	public int BaseDamage { get; set; }
	public float BaseSpeed { get; set; }

	public BaseGun(int damage, float speed)
	{
		BaseDamage = damage;
		BaseSpeed = speed;
	}

	public virtual Node2D Shoot(Node2D shooter, Vector2 direction)
	{
		if (_bulletScene == null) return null;
		
		Bullet bullet = _bulletScene.Instantiate<Bullet>(); 
		bullet.Direction = direction;
		bullet.GlobalPosition = shooter.GlobalPosition;
		
		bullet.Damage = BaseDamage;
		bullet.Speed = BaseSpeed;

		shooter.GetParent().AddChild(bullet);
		return bullet;
	}
}

public class Pistol : BaseGun { public Pistol() : base(15, 400f) { } }
public class SMG : BaseGun { public SMG() : base(5, 600f) { } } // Малий урон, але летить швидше
public class SniperRifle : BaseGun { public SniperRifle() : base(100, 1000f) { } } // Величезний урон і швидкість

public abstract class WeaponDecorator : IWeapon
{
	protected IWeapon _wrappedWeapon;
	public WeaponDecorator(IWeapon weapon) { _wrappedWeapon = weapon; }
	public virtual Node2D Shoot(Node2D startPoint, Vector2 direction) => _wrappedWeapon.Shoot(startPoint, direction);
}

public class PoisonPassive : WeaponDecorator
{
	public PoisonPassive(IWeapon weapon) : base(weapon) { }
	public override Node2D Shoot(Node2D startPoint, Vector2 direction)
	{
		Node2D b = base.Shoot(startPoint, direction);
		if (b is Bullet bullet)
		{
			bullet.Damage += 5;
			Sprite2D sprite = bullet.GetNodeOrNull<Sprite2D>("Sprite2D");
			if (sprite != null) sprite.Modulate = new Color(0, 1, 0); // Зелений
		}
		return b;
	}
}

public class HeavyBulletPassive : WeaponDecorator
{
	public HeavyBulletPassive(IWeapon weapon) : base(weapon) { }
	public override Node2D Shoot(Node2D startPoint, Vector2 direction)
	{
		Node2D b = base.Shoot(startPoint, direction);
		if (b is Bullet bullet)
		{
			bullet.Damage *= 2;
			bullet.Speed *= 0.6f; 
			Sprite2D sprite = bullet.GetNodeOrNull<Sprite2D>("Sprite2D");
			if (sprite != null) sprite.Scale *= 2.0f;
		}
		return b;
	}
}

public class SpreadShotPassive : WeaponDecorator
{
	public SpreadShotPassive(IWeapon weapon) : base(weapon) { }
	public override Node2D Shoot(Node2D startPoint, Vector2 direction)
	{
		Node2D centerBullet = base.Shoot(startPoint, direction);
		
		Vector2 leftDir = direction.Rotated(-0.2f);
		base.Shoot(startPoint, leftDir);
		
		Vector2 rightDir = direction.Rotated(0.2f);
		base.Shoot(startPoint, rightDir);

		return centerBullet;
	}
}
