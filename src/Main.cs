using Godot;

public partial class Main : Node2D
{
    private Control _pause;

    public override void _Ready()
    {
        _pause = GetNode<Control>("Pause");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Escape") && !_pause.Visible)
        {
            _pause.Visible = true;
        }
        else if (Input.IsActionJustPressed("Escape") && _pause.Visible)
        {
            _pause.Visible = false;
        }
    }

    public void OnMainMenuButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
    }

    public void OnResumeButtonPressed()
    {
        _pause.Visible = false;
    }
}
