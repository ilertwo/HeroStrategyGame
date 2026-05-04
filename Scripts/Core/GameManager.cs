using Godot;
using System;

public partial class GameManager : Node
{
	private static GameManager _instance;
	public static GameManager Instance => _instance;
	
	[Signal] public delegate void ScoreChangedEventHandler(int newScore);

	public LevelFacade CurrentLevelFacade { get; set; }

	public int Score { get; private set; } = 0;
	private int _targetScore = 50; 
	private bool _evacuationStarted = false;

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
		EmitSignal(SignalName.ScoreChanged, Score);

		if (Score >= _targetScore && !_evacuationStarted)
		{
			_evacuationStarted = true;
			
			if (CurrentLevelFacade != null)
			{
				CurrentLevelFacade.TriggerEvacuationPhase();
			}
			else
			{
				GD.PrintErr("GameManager: Фасад не зареєстровано!");
			}
		}
		}
	public void SpendScore(int amount)
	{
		Score -= amount;
		GD.Print($"Витрачено {amount} балів. Залишок: {Score}");
		EmitSignal(SignalName.ScoreChanged, Score);
	}
}
