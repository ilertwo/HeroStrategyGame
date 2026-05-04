using Godot;
using System;

public partial class ShopUI : Control
{
	private Hero _hero;

	public override void _Ready()
	{
		_hero = GetTree().GetFirstNodeInGroup("Player") as Hero;
		Hide(); 
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept")) 
		{
			Visible = !Visible;
			GetTree().Paused = Visible; 
		}
	}


	public void OnBuySMGPressed()
	{
		if (GameManager.Instance.Score >= 50) {
			GameManager.Instance.SpendScore(50);
			_hero.ShootingLogic = new SMG();
			GD.Print("Куплено: Пістолет-кулемет (SMG)!");
		} else GD.Print("Недостатньо балів!");
	}

	public void OnBuySniperPressed()
	{
		if (GameManager.Instance.Score >= 100) {
			GameManager.Instance.SpendScore(100);
			_hero.ShootingLogic = new SniperRifle();
			GD.Print("Куплено: Снайперська гвинтівка!");
		} else GD.Print("Недостатньо балів!");
	}

	public void OnBuyPoisonPressed()
	{
		if (GameManager.Instance.Score >= 50) {
			GameManager.Instance.SpendScore(50);
			_hero.ShootingLogic = new PoisonPassive(_hero.ShootingLogic);
			GD.Print("Куплено: Отруйні кулі!");
		} else GD.Print("Недостатньо балів!");
	}

	public void OnBuyHeavyBulletPressed()
	{
		if (GameManager.Instance.Score >= 75) {
			GameManager.Instance.SpendScore(75);
			_hero.ShootingLogic = new HeavyBulletPassive(_hero.ShootingLogic);
			GD.Print("Куплено: Важкі кулі!");
		} else GD.Print("Недостатньо балів!");
	}

	public void OnBuySpreadShotPressed()
	{
		if (GameManager.Instance.Score >= 150) {
			GameManager.Instance.SpendScore(150);
			_hero.ShootingLogic = new SpreadShotPassive(_hero.ShootingLogic);
			GD.Print("Куплено: Потрійний постріл!");
		} else GD.Print("Недостатньо балів!");
	}
}
