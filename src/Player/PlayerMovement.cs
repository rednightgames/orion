using System.Linq;
using Godot;

public partial class Player : CharacterBody3D
{
    private const float MinimalItemCollectDistance = 0.1f;

    public override void _PhysicsProcess(double delta)
    {
        foreach (var item in _inventory._items)
        {
            if (item != null)
            GD.Print(item.Description);
        }

        Vector3 inputDirection = new Vector3(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            0,
            Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")
        );

        if (inputDirection != Vector3.Zero)
        {
            _isMovingToItem = false;
        }

        if (inputDirection != Vector3.Zero && !_isMovingToItem)
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
            RotateCamera(-CameraRotationAngle);
        }
        else if (Input.IsActionPressed("E") && !_isCameraRotating)
        {
            RotateCamera(CameraRotationAngle);
        }

        if (
            Input.IsActionPressed("ui_accept")
            && _targetItem != null
            && !_isMovingToItem
            && inputDirection == Vector3.Zero
        )
        {
            var map = GetWorld3D().NavigationMap;
            _navigation.TargetPosition = NavigationServer3D.MapGetClosestPoint(map, _targetItem.GlobalPosition);
            _isMovingToItem = true;
        }

        if (_isMovingToItem)
        {
            if (_targetItem == null) {
                _isMovingToItem = false;
                return;
            }
            if (
                GlobalTransform.Origin.DistanceTo(_targetItem.GlobalTransform.Origin)
                < MinimalItemCollectDistance
            )
            {
                _inventory.AddItem(_targetItem.GetParent<InventoryItem>());
                _targetItem.GetParent().QueueFree();
                _targetItem = _area.GetOverlappingAreas().Last();
                _isMovingToItem = false;
                return;
            }
            var direction = _navigation.GetNextPathPosition() - GlobalPosition;
            direction = direction.Normalized();

            Velocity = new Vector3(direction.X, 0, direction.Z) * speed * (float)delta;

            _stateMachine.Travel("walk");
            _anim.Set(
                "parameters/idle/blend_position",
                new Vector3(direction.X, direction.Z, direction.Y)
            );
            _anim.Set(
                "parameters/walk/blend_position",
                new Vector3(direction.X, direction.Z, direction.Y)
            );

            MoveAndSlide();
        }
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
