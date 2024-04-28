using System;
using Godot;

public partial class MainMenu : Node2D
{
    private Control _SPMP;

    public override void _Ready()
    {
        _SPMP = GetNode<Control>("SPMP");
    }

    public void OnPlayButtonPressed()
    {
        if (!_SPMP.Visible)
        {
            _SPMP.Visible = true;
        }
        else
        {
            _SPMP.Visible = false;
        }
    }

    public void OnExitButtonPressed()
    {
        GetTree().Quit();
    }

    public void OnSPButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/Main.tscn");
    }

    public void OnMPButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/Lobby.tscn");
    }
}
