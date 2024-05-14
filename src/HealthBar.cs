using Godot;

public partial class HealthBar : Label
{
	private Player _player;
	public override void _Ready()
	{
		_player = GetParent().GetParent().GetParent().GetNode<Player>("Player");
	}

	public override void _Process(double delta)
	{
		//Text = "HP: " + _player;
	}
}
