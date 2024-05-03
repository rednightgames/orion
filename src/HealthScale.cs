using Godot;
using System;

public partial class HealthScale : Label
{
	private Player _player;
	public override void _Ready()
	{
		_player = GetParent().GetParent().GetParent().GetNode<Player>("Player");
	}

	public override void _Process(double delta)
	{
		Text = "HP: " + _player.playerHealth;
	}
}
