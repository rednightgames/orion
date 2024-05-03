using System.Collections.Generic;
using Godot;

public partial class PlayerRef : CharacterBody3D
{
    public float speed;
    private const int CameraRotationAngle = 45;
    private const float CameraRotationDuration = 0.3f;
    private AnimationTree _anim;
    private AnimationNodeStateMachinePlayback _stateMachine;
    private Vector3 _rotation;
    private Tween _cameraTween;
    private bool _isCameraRotating = false;
    private Inventory _inventory;
    private Area3D _area;
    public int playerHealth = 10;
    private HealthScale _healthScale;
    private Dictionary<string, object> _initialPropertyValues = new Dictionary<string, object> { };

    internal void Init(float speed)
    {
        this.speed = speed;

        _initialPropertyValues.Add(nameof(speed), speed);
    }

    public override void _Ready()
    {
        _inventory = new Inventory(this);
        _navigation = GetNode<NavigationAgent3D>("Navigation");
        _anim = GetNode<AnimationTree>("AnimationTree");
        _stateMachine = (AnimationNodeStateMachinePlayback)_anim.Get("parameters/playback");
        _healthScale = GetNode<HealthScale>("../UI/HealthScale");
        _area = GetNode<Area3D>("Area");
    }

    public void ResetStats()
    {
        speed = (float)_initialPropertyValues[nameof(speed)];
    }
}
