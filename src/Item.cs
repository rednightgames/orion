using System.IO;
using Godot;

public partial class Item : InventoryItem
{
    Sprite3D _sprite;
    private float _speedBoost;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite3D>("Sprite3D");
    }

    public async void Init(string name, string description, string texture, float speedBoost)
    {
        Name = name;
        Description = description;
        _speedBoost = speedBoost;

        Error error = ResourceLoader.LoadThreadedRequest(texture, typeof(Texture2D).Name);

        if (error != Error.Ok)
        {
            throw new IOException(
                $"Failed requesting to load resource at path \"{texture}\". Error: {error}."
            );
        }

        _sprite.Texture = GD.Load<Texture2D>(texture);
    }

    public override void ApplyEffect(Player player)
    {
        player.speed += _speedBoost;
    }
}
