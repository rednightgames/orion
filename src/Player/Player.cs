namespace Player;

public partial class Player : Movement
{
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    public void Reset()
    {
        _stats.Reset();
    }
}
