using Godot;

public partial class HungerBar : TextureProgressBar
{
	private Entity _entity;

	public override void _Ready()
	{
		_entity = GetParent().GetParent().GetParent().GetNode<Entity>("Entity");
		MaxValue = _entity.hungryValueMax;
		MinValue = _entity.hungryValueMin;
	}
	public override void _Process(double delta)
	{
		Value = _entity.HungryValue;
	}
}
