using Godot;
using System;

public partial class ScoreUI : Label
{
	public override void _Ready()
	{
		// 3. Підписуємося на сигнал менеджера при старті
		// Використовуємо Instance, бо це Singleton
		GameManager.Instance.ScoreChanged += OnScoreChanged;
		
		// Встановлюємо початкове значення
		Text = $"Рахунок: {GameManager.Instance.Score}";
	}

	// 4. Метод, який виконається, коли прийде сигнал
	private void OnScoreChanged(int newScore)
	{
		Text = $"Рахунок: {newScore}";
	}
}
