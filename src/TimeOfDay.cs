using Godot;

public partial class TimeOfDay : Label
{
    private DayNightCycle _dayNightCycle;

    public override void _Ready()
    {
        _dayNightCycle = GetNode<DayNightCycle>("../../DayNightCycle");
    }

    public override void _Process(double delta)
    {
        Text = _dayNightCycle._timeDay;
    }
}
