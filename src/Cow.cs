using Godot;

public partial class Cow : CharacterBody3D
{
    private enum State
    {
        Moving,
        Waiting
    }

    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _targetPosition;
    private State _currentState = State.Moving;
    private AnimationPlayer _animationPlayer;
    private DayNightCycle _dayNightCycle;
    private float _waitTime = 2.0f; // Adjust the wait time as needed
    private float _waitStartTime = 0.0f;

    public float MoveSpeed = 80.0f;
    public float WanderRadius = 2.0f;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _dayNightCycle = GetParent().GetNode<DayNightCycle>("DayNightCycle");
        _targetPosition = GetNewTargetPosition();
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (_currentState)
        {
            case State.Moving:
                MoveToTarget((float)delta);
                _animationPlayer.Play("walk");
                break;
            case State.Waiting:
                WaitForSeconds();
                _animationPlayer.Play("idle");
                break;
        }
    }

    private void MoveToTarget(float delta)
    {
        Vector3 direction = (_targetPosition - GlobalTransform.Origin).Normalized();
        Velocity = direction * MoveSpeed * delta;

        // Check if the cow has reached the target position
        if (GlobalTransform.Origin.DistanceTo(_targetPosition) < 0.1f)
        {
            _currentState = State.Waiting;
            _waitStartTime = (float)_dayNightCycle.leveltime;
        }

        MoveAndSlide();
    }

    // AI logic for waiting
    private void WaitForSeconds()
    {
        if (_dayNightCycle.leveltime >= _waitStartTime + _waitTime)
        {
            // Wait time elapsed, choose a new target position and resume moving
            _targetPosition = GetNewTargetPosition();
            _currentState = State.Moving;
        }
    }

    // Generate a random point within the wander radius
    private Vector3 GetNewTargetPosition()
    {
        return GlobalTransform.Origin + new Vector3(
            (float)GD.RandRange(-WanderRadius, WanderRadius),
            0.0f,
            (float)GD.RandRange(-WanderRadius, WanderRadius)
        );
    }
}