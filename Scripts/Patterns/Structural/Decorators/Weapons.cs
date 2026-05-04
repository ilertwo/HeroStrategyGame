using Godot;
using System;

public interface IWeapon
{
	Node2D Shoot(Node2D startPoint, Vector2 direction);
}

public class BasicGun : IWeapon
{
	private PackedScene _bulletScene;

	public BasicGun()
	{
		_bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet.tscn");
	}

	public Node2D Shoot(Node2D shooter, Vector2 direction)
	{
		if (_bulletScene == null) return null;

		Bullet bullet = _bulletScene.Instantiate<Bullet>(); 

		bullet.Direction = direction;
		bullet.GlobalPosition = shooter.GlobalPosition;

		shooter.GetParent().AddChild(bullet);
		
		return bullet; 
	}
}

public abstract class WeaponDecorator : IWeapon
{
	protected IWeapon _wrappedWeapon;

	public WeaponDecorator(IWeapon weapon)
	{
		_wrappedWeapon = weapon;
	}

	public virtual Node2D Shoot(Node2D startPoint, Vector2 direction)
	{
		return _wrappedWeapon.Shoot(startPoint, direction);
	}
}

public class FireBulletDecorator : WeaponDecorator
{
	public FireBulletDecorator(IWeapon weapon) : base(weapon) { }

	public override Node2D Shoot(Node2D startPoint, Vector2 direction)
	{
		Node2D spawnedBullet = base.Shoot(startPoint, direction);

		if (spawnedBullet == null) return null;

		if (GD.Randf() <= 0.4f)
		{
			GD.Print("ЕФЕКТ: Вилетіла ВОГНЯНА куля!");

			Sprite2D sprite = spawnedBullet.GetNodeOrNull<Sprite2D>("Sprite2D");
			
			if (sprite != null)
			{
				sprite.Modulate = new Color(1, 0, 0); 
			}
			else if (spawnedBullet is CanvasItem canvasItem)
			{
				canvasItem.Modulate = new Color(1, 0, 0);
			}
		}

		return spawnedBullet;
	}
}
