using Godot;

public partial class HungerScale : ProgressBar
{
	private Entity _entity;

	public override void _Ready()
	{
		_entity = GetNode<Entity>("../../Entity");
		MaxValue = _entity.hungryValueMax;
		MinValue = _entity.hungryValueMin;
	}
	public override void _Process(double delta)
	{
		Value = _entity.hungryValue;
	}
}
