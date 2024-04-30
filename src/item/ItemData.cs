using Godot.Collections;

public class ItemData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Texture { get; set; }

    public ItemData(Dictionary from)
    {
        Name = from["name"].As<string>();
        Description = from["description"].As<string>();
        Texture = PathManager.ResolvePath(from["texture"].As<string>());
    }
}
