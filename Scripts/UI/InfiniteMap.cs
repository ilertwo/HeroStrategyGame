using Godot;
using System;

public partial class InfiniteMap : Sprite2D
{
	private Node2D _player;

	public override void _Ready()
	{
		// Знаходимо героя автоматично
		_player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		ZIndex = -100; // Кидаємо під низ
	}

	public override void _Process(double delta)
	{
		if (_player == null) return;

		// 1. Позиція йде за гравцем, але ПОВОРОТ жорстко залишається нульовим
		GlobalPosition = _player.GlobalPosition;
		GlobalRotation = 0; 

		// 2. Скролимо саму картинку всередині спрайта
		RegionRect = new Rect2(GlobalPosition, RegionRect.Size);
	}
}
