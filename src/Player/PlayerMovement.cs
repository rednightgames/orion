using Godot;

public partial class Player : CharacterBody3D
{
    public override void _PhysicsProcess(double delta)
    {
        Vector3 inputDirection = new Vector3(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            0,
            Input.GetActionStrength("move_backward") - Input.GetActionStrength("move_forward")
        );

        if (inputDirection != Vector3.Zero)
        {
            _isMovingToItem = false;

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

        if (Input.IsActionJustPressed("ui_accept") && _targetItem != null)
        {
            GD.Print("ui_accept");
            _navigation.TargetPosition = _targetItem.GlobalTransform.Origin;
            _isMovingToItem = true;
        }

        if (_isMovingToItem)
        {
            var nextPathPosition = _navigation.GetNextPathPosition();
            var newVelocity = GlobalPosition.DirectionTo(nextPathPosition) * speed * (float)delta;

            _navigation.SetVelocityForced(newVelocity);

            MoveAndSlide();
        }
    }

    private void _on_velocity_computed(Vector3 safe_velocity, double delta)
    {
        GlobalPosition = GlobalPosition.MoveToward(GlobalPosition + safe_velocity, (float)delta);
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
