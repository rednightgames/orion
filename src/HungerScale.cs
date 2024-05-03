using Godot;

public partial class HungerScale : TextureProgressBar
{
	private Entity _entity;

	public override void _Ready()
	{
		//_entity = GetNode<Entity>("/root/Level/Entity");
		_entity = GetParent().GetParent().GetParent().GetNode<Entity>("Entity");
		MaxValue = _entity.hungryValueMax;
		MinValue = _entity.hungryValueMin;
	}
	public override void _Process(double delta)
	{
		Value = _entity.hungryValue;
	}
}
