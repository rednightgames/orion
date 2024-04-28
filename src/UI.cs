using Godot;

public partial class UI : Control
{
    private ProgressBar _hungerScale;
    private Entity _entity;

    public override void _Ready()
    {
        _hungerScale = GetNode<ProgressBar>("HungerScale");
        _entity = GetNode<Entity>("../Entity");
    }

    public override void _Process(double delta)
    {
        _hungerScale.Value = _entity._hungryValue;
    }
}
