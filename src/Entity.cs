using Godot;

public partial class Entity : Node3D
{
    [Export]
    public float hungryValueMax = 100,
        hungryValueMin = 0;
    private float _hungryValue = 100,
        hungryChange = 1,
        hungryBoost = 1;
    public float HungryValue {
        set {
            if (value < 0) {
                _hungryValue = hungryValueMin;
            }
            else if (value > hungryValueMax) {
                _hungryValue = hungryValueMax;
            }
            else {
                _hungryValue = value;
            }
        }
        get {
            return _hungryValue;
        }
    }
    public void EntityHungryTimer()
    {
        HungryValue -= hungryChange * hungryBoost;
    }
}
