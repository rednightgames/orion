using System;
using Godot;
using Player;
using ResourceManager;

public partial class Item : InventoryItem
{
    [Export]
    public string Id;
    public event Action OnInit;
    private Sprite3D _sprite;

    public Item() { }

    public override void _Ready()
    {
        _sprite = GetNode<Sprite3D>("Sprite");
        OnInit?.Invoke();
        PostInit();
    }

    protected void PostInit()
    {
        TextureResource data = AssetManager.Load<TextureResource>(Id);
        Name = data.Name;
        Description = data.Description;
        _sprite.Texture = data.Resource;
    }
}
