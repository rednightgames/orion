using Godot;

public partial class CounterDay : Label
{
    private DayNightCycle _dayNightCycle;

    public override void _Ready()
    {
        _dayNightCycle = GetNode<DayNightCycle>("../../DayNightCycle");
    }

    public override void _Process(double delta)
    {
        Text = (_dayNightCycle.countday).ToString();
    }
}
