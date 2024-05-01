using Godot;

public partial class Main : Node3D
{
    [Export]
    public Vector3 SpawnPosition = new Vector3(0, 0, 0);
    public Vector3 SpawnItemPosition = new Vector3(1, 0, 1);
    AssetManager ResourceManager;

    public override void _Ready()
    {
        ResourceManager = GetNode<AssetManager>("/root/AssetManager");
        Create_player();
        Spawn_Items();
    }

    public override void _Process(double delta) { }

    public void Create_player()
    {
        Player player = GD.Load<PackedScene>("res://scenes/Player.tscn").Instantiate<Player>();
        player.Position = SpawnPosition;
        player.Init(160.0f);
        AddChild(player);
    }

    public void Spawn_Items()
    {
        ItemData data = ResourceManager.Load<ItemData>("grass");
        Item item = GD.Load<PackedScene>("res://scenes/Item.tscn").Instantiate<Item>();
        GD.Print("data.Texture  ", data.Texture);
        GD.Print("data.Resource ", data.Resource);
        item.Position = SpawnItemPosition;
        item.Init(data.Name, data.Name, data.Texture, 160);
        AddChild(item);
    }
}
