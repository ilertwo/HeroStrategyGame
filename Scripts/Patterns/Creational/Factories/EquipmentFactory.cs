using Godot;
using System;


public interface IFactoryWeapon 
{
	void Attack();
}

public interface IArmor 
{
	void Protect();
}

public class MagicStaff : IFactoryWeapon
{
	public void Attack() => GD.Print("Випущено вогняну кулю!");
}

public class MageRobe : IArmor 
{
	public void Protect() => GD.Print("");
}

public class HeavySword : IFactoryWeapon
{
	public void Attack() => GD.Print("");
}

public class PlateArmor : IArmor 
{
	public void Protect() => GD.Print("Сталева броня блокує фізичний удар!");
}

// АБСТРАКТНА ФАБРИКА
public interface IEquipmentFactory 
{
	IFactoryWeapon CreateWeapon(); 
	IArmor CreateArmor();
}

public class MagicDevelopmentFactory : IEquipmentFactory 
{
	public IFactoryWeapon CreateWeapon() => new MagicStaff(); 
	public IArmor CreateArmor() => new MageRobe();
}

public class StrengthDevelopmentFactory : IEquipmentFactory 
{
	public IFactoryWeapon CreateWeapon() => new HeavySword();
	public IArmor CreateArmor() => new PlateArmor();
}
