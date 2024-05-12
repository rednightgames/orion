using Godot;

public partial class Main : Node3D
{
    [Export]
    public Vector3 SpawnPosition = new Vector3(0, 0, 0);
    public Vector3 SpawnItemPosition = new Vector3(1, 0, 1);

    public override void _Ready()
    {
        Create_player();
        Spawn_Items();
    }

    public override void _Process(double delta) 
    {
     }

    public void Create_player()
    {
        Player player = GD.Load<PackedScene>("res://scenes/Player.tscn").Instantiate<Player>();
        player.Position = SpawnPosition;
        player.Init(160.0f);
        AddChild(player);
    }

    public void Spawn_Items()
    {
        Item item = ResourceManager.ResourceManager.LoadItem<Item>("grass");
        Item item2 = ResourceManager.ResourceManager.LoadItem<Item>("egg");
        item.Position = SpawnItemPosition;
        item2.Position = new Vector3(1, 0, -1);
        AddChild(item);
        AddChild(item2);
    }
}
