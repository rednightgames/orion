using Godot;

public partial class TimeOfDay : Godot.TextureRect
{
    private Texture2D _day,
        _evening,
        _night;
    private DayNightCycle _dayNightCycle;

    public override void _Ready()
    {
        _day = GD.Load<Texture2D>("res://assets/day.png");
        _evening = GD.Load<Texture2D>("res://assets/evening.png");
        _night = GD.Load<Texture2D>("res://assets/night.png");
        _dayNightCycle = GetParent()
            .GetParent()
            .GetParent()
            .GetNode<DayNightCycle>("DayNightCycle");
    }

    public override void _Process(double delta)
    {
        SwapIcon();
    }

    private void SwapIcon()
    {
        if (_dayNightCycle._timeDay == "DAY")
        {
            Texture = _day;
        }
        if (_dayNightCycle._timeDay == "EVENING")
        {
            Texture = _evening;
        }
        if (_dayNightCycle._timeDay == "NIGHT")
        {
            Texture = _night;
        }
    }
}
