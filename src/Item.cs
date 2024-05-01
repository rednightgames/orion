using System;
using Godot;

public partial class Item : InventoryItem
{
    public event Action Initialized;
    private Sprite3D _sprite;
    private float _speedBoost;

    public override void _Ready()
    {
        _sprite = GetNode<Sprite3D>("Sprite");
        GD.Print("_Ready._sprite ", _sprite);
        OnInitialized();
    }

    protected virtual void OnInitialized()
    {
        Initialized?.Invoke();
    }

    public void Init(string name, string description, Texture2D texture, float speedBoost)
    {
        Name = name;
        Description = description;
        _speedBoost = speedBoost;
        _sprite.Texture = texture;
    }

    public override void ApplyEffect(Player player)
    {
        player.speed += _speedBoost;
    }
}
