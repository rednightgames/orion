using System.Collections.Generic;
using Godot;

public partial class Player : CharacterBody3D
{
    public float speed;
    private const int CameraRotationAngle = 45;
    private const float CameraRotationDuration = 0.3f;
    private AnimationTree _anim;
    private AnimationNodeStateMachinePlayback _stateMachine;
    private PlayerCamera _playerCamera;
    private Vector3 _rotation;
    private Tween _cameraTween;
    private bool _isCameraRotating = false;
    private Inventory _inventory;
    private Area3D _area;
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
        _playerCamera = GetNode<PlayerCamera>("PlayerCamera");
        _area = GetNode<Area3D>("Area");
        Control ui = GD.Load<PackedScene>("res://scenes/UI.tscn").Instantiate<Control>();
        AddChild(ui);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            switch (mouseEvent.ButtonIndex)
            {
                case MouseButton.WheelUp:
                    _playerCamera.SetCameraZoom(_playerCamera.Zoom * 1.2f);
                    break;
                case MouseButton.WheelDown:
                    _playerCamera.SetCameraZoom(_playerCamera.Zoom * 0.8f);
                    break;
            }
        }
    }

    public void ResetStats()
    {
        speed = (float)_initialPropertyValues[nameof(speed)];
    }
}
