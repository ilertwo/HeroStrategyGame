using Godot;
using System;

public partial class HeroStatsController : Control
{
	[Export] public NodePath HeroPath; 
	private Hero _hero;

	public override void _Ready()
	{
		if (HeroPath != null)
		{
			_hero = GetNode<Hero>(HeroPath);
			_hero.HealthChanged += OnHeroHealthChanged;
			
			GetNode<Label>("Label").Text = $"HP: {_hero.Health}";
		}
	}

	private void OnHeroHealthChanged(int newHealth)
	{
		GetNode<Label>("Label").Text = $"HP: {newHealth}";
	}
}
