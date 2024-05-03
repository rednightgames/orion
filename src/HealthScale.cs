using System;
using Godot;

public partial class HealthScale : Label
{
    public int playerHealth;

    public override void _Ready() { }

    public override void _Process(double delta)
    {
        Text = "HP: " + playerHealth;
    }
}
