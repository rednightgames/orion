using Godot;

public partial class Player : CharacterBody3D
{
    public float speed = 160.0f;
    private AnimationTree Anim;
    private AnimationNodeStateMachinePlayback StateMachine;
    private PlayerCamera Camera;
    private Vector3 _rotation;

    public override void _Ready()
    {
        Camera = GetNode<PlayerCamera>("PlayerCamera");
        Anim = GetNode<AnimationTree>("AnimationTree");
        StateMachine = (AnimationNodeStateMachinePlayback)Anim.Get("parameters/playback");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            switch (mouseEvent.ButtonIndex)
            {
                case MouseButton.WheelUp:
                {
                    Camera.SetCameraZoom(Camera.Zoom * 1.2f);
                    break;
                }
                case MouseButton.WheelDown:
                {
                    Camera.SetCameraZoom(Camera.Zoom * 0.8f);
                    break;
                }
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

        Vector3 rotatedInputDirection = inputDirection.Rotated(Vector3.Up, Rotation.Y).Normalized();

        inputDirection = inputDirection.Normalized();

        Velocity = rotatedInputDirection * speed * (float)delta;

        if (Input.IsActionJustPressed("Q"))
        {
            _rotation = new Vector3(0, Rotation.Y - 45 * Mathf.Pi / 180, 0);
        }
        if (Input.IsActionJustPressed("E"))
        {
            _rotation = new Vector3(0, Rotation.Y + 45 * Mathf.Pi / 180, 0);
        }
        Tween tween_rotation = GetTree().CreateTween();
        tween_rotation.TweenProperty(this, "rotation", _rotation, 0.3);

        if (inputDirection == Vector3.Zero)
        {
            StateMachine.Travel("idle");
        }
        else
        {
            StateMachine.Travel("walk");
            Anim.Set(
                "parameters/idle/blend_position",
                new Vector3(inputDirection.X, inputDirection.Z, inputDirection.Y)
            );
            Anim.Set(
                "parameters/walk/blend_position",
                new Vector3(inputDirection.X, inputDirection.Z, inputDirection.Y)
            );

            MoveAndSlide();
        }
    }
}
