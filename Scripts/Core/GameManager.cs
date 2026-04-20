using Godot;
using System;

public partial class GameManager : Node
{
	private static GameManager _instance;
	public static GameManager Instance => _instance;

	public int Score { get; private set; } = 0;

	public override void _Ready()
	{
		if (_instance != null)
		{
			QueueFree();
			return;
		}
		_instance = this;
	}

	public void AddScore(int amount)
	{
		Score += amount;
		GD.Print($"Рахунок: {Score}");
	}
}
