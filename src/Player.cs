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
    private Dictionary<string, object> _initialPropertyValues = new Dictionary<string, object> { };

    internal void Init(float speed)
    {
        this.speed = speed;

        _initialPropertyValues.Add(nameof(speed), speed);
    }

    public override void _Ready()
    {
        _inventory = new Inventory(this);
        _anim = GetNode<AnimationTree>("AnimationTree");
        _stateMachine = (AnimationNodeStateMachinePlayback)_anim.Get("parameters/playback");
        _playerCamera = GetNode<PlayerCamera>("PlayerCamera");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            switch (mouseEvent.ButtonIndex)
            {
                case MouseButton.WheelUp:
                    _inventory.SetActiveSlot(4);
                    _playerCamera.SetCameraZoom(_playerCamera.Zoom * 1.2f);
                    break;
                case MouseButton.WheelDown:
                    _inventory.SetActiveSlot(3);
                    _playerCamera.SetCameraZoom(_playerCamera.Zoom * 0.8f);
                    break;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 inputDirection = new Vector3(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            0,
            Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")
        );

        if (inputDirection != Vector3.Zero)
        {
            Vector3 rotatedInputDirection = inputDirection
                .Rotated(Vector3.Up, Rotation.Y)
                .Normalized();
            inputDirection = inputDirection.Normalized();
            Velocity = rotatedInputDirection * speed * (float)delta;

            _stateMachine.Travel("walk");
            _anim.Set(
                "parameters/idle/blend_position",
                new Vector3(inputDirection.X, inputDirection.Z, inputDirection.Y)
            );
            _anim.Set(
                "parameters/walk/blend_position",
                new Vector3(inputDirection.X, inputDirection.Z, inputDirection.Y)
            );

            MoveAndSlide();
        }
        else
        {
            _stateMachine.Travel("idle");
        }

        if (Input.IsActionPressed("Q") && !_isCameraRotating)
        {
            _inventory.SetActiveSlot(0);
            RotateCamera(-CameraRotationAngle);
        }
        else if (Input.IsActionPressed("E") && !_isCameraRotating)
        {
            _inventory.SetActiveSlot(1);
            RotateCamera(CameraRotationAngle);
        }
    }

    public void ResetStats()
    {
        speed = (float)_initialPropertyValues[nameof(speed)];
    }

    private void RotateCamera(float angle)
    {
        _rotation.Y += Mathf.DegToRad(angle);
        // Dispose existing tween before creating a new one
        DisposeCameraTween();
        _cameraTween = CreateTween().SetParallel(true);
        // Tween camera rotation
        _cameraTween.TweenProperty(
            this,
            "rotation",
            new Vector3(Rotation.X, _rotation.Y, Rotation.Z),
            CameraRotationDuration
        );
        _isCameraRotating = true;
        _cameraTween.Connect("finished", new Callable(this, nameof(OnCameraRotationCompleted)));
    }

    private void DisposeCameraTween()
    {
        _cameraTween?.Dispose();
        _cameraTween = null;
    }

    private void OnCameraRotationCompleted()
    {
        _isCameraRotating = false;
        _rotation.Y = Mathf.Wrap(Rotation.Y, -Mathf.Pi, Mathf.Pi);
        Rotation = new Vector3(Rotation.X, _rotation.Y, Rotation.Z);
    }
}
