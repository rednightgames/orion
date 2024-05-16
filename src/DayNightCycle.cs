using Godot;

public partial class DayNightCycle : Node
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
    public double leveltime;
    private int cycletimer;
    private int durationcycle;
    public int countday = 1;
    private DirectionalLight3D _dirLight;
    private WorldEnvironment _worldEnv;
    private Tween _dirLightTween;
    private Tween _worldEnvTween;
    private double _transition = 1;
    public string _timeDay;
    public Timer timer;

    public override void _Ready()
    {
        leveltime = 0f;
        cycletimer = 0;
        durationcycle = durationday + durationevening + durationnight;
        _dirLight = GetNode<DirectionalLight3D>("../DirectionalLight3D");
        _worldEnv = GetNode<WorldEnvironment>("../WorldEnvironment");
        timer = new Timer
        {
            Name = "timer",
            WaitTime = 1,
            OneShot = false,
            Autostart = true
        };
        AddChild(timer);
        timer.Timeout += TimerTimeout;
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

    public void TimerTimeout()
    {
        leveltime++;
        cycletimer++;
    }

    public void day_state()
    {
        _timeDay = "DAY";
        TransitionLight(1, 1);
    }

    public void evening_state()
    {
        _timeDay = "EVENING";
        TransitionLight(0.3, 0.3);
    }

    public void night_state()
    {
        _timeDay = "NIGHT";
        TransitionLight(0.01, 0);
    }

    public void ChangeState(TimeState newState)
    {
        state = newState;
    }

    public void TransitionLight(double lightEnergy, double bgEnergy)
    {
        _dirLightTween = CreateTween();
        _dirLightTween.TweenProperty(_dirLight, "light_energy", lightEnergy, _transition);
        _worldEnvTween = CreateTween();
        _worldEnvTween.TweenProperty(
            _worldEnv.Environment,
            "background_energy_multiplier",
            lightEnergy,
            _transition
        );
    }
}
