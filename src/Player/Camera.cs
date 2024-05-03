namespace Player;

using Godot;

public partial class Camera : Camera3D
{
    public float Zoom = 1.0f;
    private float zoomSpeed = 0.1f;
    private const float MinCameraZoom = 0.1f;
    private const float MaxCameraZoom = 3f;
    private static int CameraRotationAngle = 45;
    private static float CameraRotationDuration = 0.3f;
    private float _targetZoom = 1.0f;
    private Sprite3D _player;
    private Vector3 _rotation;
    private Tween _cameraTween;

    private bool _isCameraRotating = false;

    public override void _Ready()
    {
        _player = GetNode<Sprite3D>("%Sprite3D");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Zoom != _targetZoom)
        {
            float newZoom = Mathf.Lerp(Zoom, _targetZoom, zoomSpeed);
            _targetZoom = Zoom;
            SetCameraZoom(newZoom);
        }

        if (Input.IsActionPressed("Q") && !_isCameraRotating)
        {
            RotateCamera(-CameraRotationAngle);
        }
        else if (Input.IsActionPressed("E") && !_isCameraRotating)
        {
            RotateCamera(CameraRotationAngle);
        }

        LookAt(_player.GlobalTransform.Origin, Vector3.Up);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            switch (mouseEvent.ButtonIndex)
            {
                case MouseButton.WheelUp:
                    SetCameraZoom(Zoom * 1.2f);
                    break;
                case MouseButton.WheelDown:
                    SetCameraZoom(Zoom * 0.8f);
                    break;
            }
        }
    }

    public void SetCameraZoom(float zoom)
    {
        Zoom = Mathf.Clamp(zoom, MinCameraZoom, MaxCameraZoom);
        Fov = Mathf.RadToDeg(Mathf.Atan(1 / zoom));
    }

    private void RotateCamera(float angle)
    {
        _rotation.Y += Mathf.DegToRad(angle);
        DisposeCameraTween();
        _cameraTween = CreateTween().SetParallel(true);
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
