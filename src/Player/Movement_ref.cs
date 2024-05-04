namespace Player;

using Godot;

public partial class MovementREF : CharacterBody3D
{
    [Export]
    public string id;
    private static float MinimalItemCollectDistance = 0.1f;
    private AnimationTree _anim;
    private AnimationNodeStateMachinePlayback _state;
    protected Node3D _targetItem;
    private bool _isMovingToItem = false;
    private NavigationAgent3D _navigation;
    private protected Stats _stats;

    public override void _Ready()
    {
        _stats = new(id);
        _anim = GetNode<AnimationTree>("AnimationTree");
        _state = (AnimationNodeStateMachinePlayback)_anim.Get("parameters/playback");
        _navigation = GetNode<NavigationAgent3D>("Navigation");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 inputDirection =
            new(
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
            Velocity = rotatedInputDirection * _stats.speed * (float)delta;

            _state.Travel("walk");
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
            _state.Travel("idle");
        }
    }
}
