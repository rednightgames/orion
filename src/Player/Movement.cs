namespace Player;

using Godot;

public class Movement
{
    private Player _player;
    private Stats _stats;
    private ItemPickUp _itemPickUp;
    private AnimationTree _anim;
    private AnimationNodeStateMachinePlayback _state;

    public Movement(Player player, Stats stats, ItemPickUp itemPickUp, AnimationTree anim)
    {
        _player = player;
        _stats = stats;
        _itemPickUp = itemPickUp;
        _anim = anim;
        _state = (AnimationNodeStateMachinePlayback)anim.Get("parameters/playback");
    }

    public void Moving(double delta)
    {
        Vector3 inputDirection =
            new(
                Input.GetAxis("move_left", "move_right"),
                0,
                Input.GetAxis("move_forward", "move_backward")
            );

        if (inputDirection != Vector3.Zero)
        {
            _itemPickUp._isMovingToItem = false;
        }

        if (inputDirection != Vector3.Zero && !_itemPickUp._isMovingToItem)
        {
            Vector3 rotatedInputDirection = inputDirection
                .Rotated(Vector3.Up, _player.Rotation.Y)
                .Normalized();
            inputDirection = inputDirection.Normalized();
            _player.Velocity = rotatedInputDirection * _stats.speed * (float)delta;

            _state.Travel("walk");
            _anim.Set(
                "parameters/idle/blend_position",
                new Vector3(inputDirection.X, inputDirection.Z, inputDirection.Y)
            );
            _anim.Set(
                "parameters/walk/blend_position",
                new Vector3(inputDirection.X, inputDirection.Z, inputDirection.Y)
            );

            _player.MoveAndSlide();
        }
        else
        {
            _state.Travel("idle");
        }
    }
}