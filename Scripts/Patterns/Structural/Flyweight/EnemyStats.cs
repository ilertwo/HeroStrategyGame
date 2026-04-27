using Godot;
using System;

[GlobalClass] 
public partial class EnemyStats : Resource
{
	[Export] public int MaxHealth { get; set; } = 30;
	[Export] public float Speed { get; set; } = 200.0f;
	[Export] public int Damage { get; set; } = 10;
}
