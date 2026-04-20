using Godot;
using System;

// Базовий компонент
public interface IWeapon
{
	void Shoot(Node2D startPoint, Vector2 direction);
}

// Конкретний компонент (Базова стрільба героя)
public class BasicTear : IWeapon
{
	public void Shoot(Node2D startPoint, Vector2 direction)
	{
		GD.Print($"Стріляю звичайною сльозою в напрямку {direction}");
		// Тут логіка інстанціювання сцени кулі в Godot
	}
}

// Базовий декоратор
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

// Конкретний декоратор (Вогняні сльози)
public class FireTearDecorator : WeaponDecorator
{
	public FireTearDecorator(IWeapon weapon) : base(weapon) { }

	public override void Shoot(Node2D startPoint, Vector2 direction)
	{
		base.Shoot(startPoint, direction);
		GD.Print("ЕФЕКТ: Сльоза підпалює ворога!");
	}
}
