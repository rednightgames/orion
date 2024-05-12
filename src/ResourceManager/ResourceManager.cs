namespace ResourceManager;

using Godot;

public static class ResourceManager
{
    public static T LoadItem<T>(string id)
        where T : Item, new()
    {
        T item = GD.Load<PackedScene>("res://scenes/Item.tscn").Instantiate<T>();
        item.OnInit += () =>
        {
            item.Id = id;
        };

        return item;
    }
}
