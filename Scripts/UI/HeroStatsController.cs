using Godot;
using System;

public partial class HeroStatsController : Control
{
	private Hero _hero;

	public override void _Ready()
	{
		// Знаходимо героя автоматично за групою (без милиць з NodePath)
		_hero = GetTree().GetFirstNodeInGroup("Player") as Hero;

		if (_hero != null)
		{
			// Підписуємося на сигнал
			_hero.HealthChanged += OnHeroHealthChanged;
			
			// Ставимо стартове ХП
			GetNode<Label>("Label").Text = $"HP: {_hero.Health}";
		}
		else
		{
			GD.PrintErr("UI не зміг знайти героя на сцені!");
		}
	}

	private void OnHeroHealthChanged(int newHealth)
	{
		GetNode<Label>("Label").Text = $"HP: {newHealth}";
	}
}
