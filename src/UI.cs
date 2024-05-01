using Godot;

public partial class UI : Control
{
    private ProgressBar _hungerScale;
    private Entity _entity;

    //private Label _timeDayUI;
    private DayNightCycle _dayNightCycle;

    public override void _Ready()
    {
        _hungerScale = GetNode<ProgressBar>("HungerScale");
        _entity = GetNode<Entity>("../Entity");
        //_timeDayUI = GetNode<Label>("TimeOfDay");
        _dayNightCycle = GetNode<DayNightCycle>("../DayNightCycle");
    }

    public override void _Process(double delta)
    {
        //GD.Print(_timeDayUI);
        _hungerScale.Value = _entity._hungryValue;
        //_timeDayUI.Text = _dayNightCycle._timeDay;
    }
}
