using Godot;
using System;

public partial class EvacuationGate : Area2D
{
	public enum GateType { MagicUpgrade, EliteBoss, Heal }
	public GateType CurrentType { get; private set; }

	private Timer _holdTimer;
	private Sprite2D _sprite;

	public override void _Ready()
	{
		AddToGroup("Gates");
	}

	public void Setup(GateType type)
	{
		CurrentType = type;
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_holdTimer = GetNode<Timer>("HoldTimer");

		// Зміна кольору
		switch (CurrentType)
		{
			case GateType.MagicUpgrade:
				_sprite.Modulate = Colors.Blue; // MagicDevelopmentFactory
				break;
			case GateType.EliteBoss:
				_sprite.Modulate = Colors.Red;  // Багато очок, але небезпечно
				break;
			case GateType.Heal:
				_sprite.Modulate = Colors.Green; // Відновлення Health
				break;
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			GD.Print($"Початок евакуації у ворота: {CurrentType}!");
			_holdTimer.Start();
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			GD.Print("Гравець вийшов із зони! Евакуація скасована.");
			_holdTimer.Stop();
		}
	}

	private void OnHoldTimerTimeout()
	{
		GD.Print($"Успішна евакуація! Перехід на рівень: {CurrentType}");
	}
}
