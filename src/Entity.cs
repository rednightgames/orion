using Godot;

public partial class Entity : Node3D
{
    [Export]
    public float _hungryValue = 100,
        _hungryBoost = 1,
        _hungryChange = 0.1f;

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public void EntityHungryTimer()
    {
        _hungryValue -= _hungryChange * _hungryBoost;
    }
}
