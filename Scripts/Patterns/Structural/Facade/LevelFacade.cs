using Godot;
using System;

public partial class LevelFacade : Node
{
	[Export] private LevelController _levelController;

	public override void _Ready()
	{
		if (GameManager.Instance != null)
		{
			GameManager.Instance.CurrentLevelFacade = this;
			GD.Print("LevelFacade: Успішно зареєстровано в GameManager!");
		}
	}

	public void TriggerEvacuationPhase()
	{
		GD.Print("ФАСАД: Запуск протоколу евакуації!");

		if (_levelController != null)
		{
			_levelController.SpawnEvacuationGates();
		}
		else
		{
			GD.PrintErr("ФАСАД: LevelController не підключений в Інспекторі!");
		}
	}
}
