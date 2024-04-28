using System;
using Godot;

public partial class Pause : Control
{
    public override void _Ready() { }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Escape"))
        {
            var pause_state = !GetTree().Paused;
            GetTree().Paused = pause_state;
            Visible = pause_state;
        }
    }

    public void _on_resume_pressed()
    {
        GetTree().Paused = false;
        Visible = false;
    }

    public void _on_exit_pressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
    }

    public override void _Process(double delta) { }
}
