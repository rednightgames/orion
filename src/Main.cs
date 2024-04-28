using Godot;

public partial class Main : Node3D
{
    [Export]
    public Vector3 SpawnPosition = new Vector3(0, 0, 0);

    public override void _Ready()
    {
        Create_player();
    }

    public override void _Process(double delta) { }

    public void Create_player()
    {
        Player player = GD.Load<PackedScene>("res://scenes/Player.tscn").Instantiate<Player>();
        player.Position = SpawnPosition;
        AddChild(player);
    }
}
