using Godot;
using System;

public partial class HeroStatsController : Control
{
	[Export] public NodePath HeroPath; // Шлях до ноди Героя в інспекторі
	private Hero _hero;

	public override void _Ready()
	{
		if (HeroPath != null)
		{
			_hero = GetNode<Hero>(HeroPath);
			// Підписуємось на подію (Observer Pattern)
			_hero.HealthChanged += OnHeroHealthChanged;
			
			// Встановлюємо початкове значення
			GetNode<Label>("Label").Text = $"HP: {_hero.Health}";
		}
	}

	private void OnHeroHealthChanged(int newHealth)
	{
		GetNode<Label>("Label").Text = $"HP: {newHealth}";
	}
}
