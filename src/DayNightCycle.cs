using Godot;

public partial class DayNightCycle : Node3D
{
    public enum TimeState
    {
        DAY,
        EVENING,
        NIGHT
    }

    [Export]
    public int durationday = 30,
        durationevening = 20,
        durationnight = 10;
    private TimeState state;
    private int leveltime;
    private int cycletimer;
    private int durationcycle;
    public int countday = 1;

    public override void _Ready()
    {
        leveltime = 0;
        cycletimer = 0;
        durationcycle = durationday + durationevening + durationnight;
    }

    public override void _Process(double delta)
    {
        if (cycletimer > durationcycle)
        {
            cycletimer = 0;
            countday++;
        }
        if ((cycletimer == 0) && durationday != 0)
        {
            ChangeState(TimeState.DAY);
        }
        if ((cycletimer == durationday) && durationevening != 0)
        {
            ChangeState(TimeState.EVENING);
        }
        if ((cycletimer == durationday + durationevening) && durationnight != 0)
        {
            ChangeState(TimeState.NIGHT);
        }
        switch (state)
        {
            case TimeState.DAY:
                day_state();
                break;
            case TimeState.EVENING:
                evening_state();
                break;
            case TimeState.NIGHT:
                night_state();
                break;
        }
    }

    public void _on_level_timer_timeout()
    {
        leveltime++;
        cycletimer++;
    }

    public void day_state() { }

    public void evening_state() { }

    public void night_state() { }

    public void ChangeState(TimeState newState)
    {
        state = newState;
    }
}
