using Godot;

public partial class Entity : Node3D
{
    [Export]
    public float hungryValueMax = 100,
        hungryValueMin = 0,
        hungryValue = 100,
        hungryBoost = 1,
        hungryChange = 1;

    private float _hungryValue
    {
        get { return hungryValue; }
        set
        {
            if ((value > hungryValueMin) && (value < hungryValueMax))
            {
                hungryValue = value;
            }
        }
    }

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public void EntityHungryTimer()
    {
        hungryValue -= hungryChange * hungryBoost;
    }
}
