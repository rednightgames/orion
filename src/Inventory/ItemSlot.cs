namespace Inventory;

using Godot;

[Tool]
public partial class ItemSlot : ColorRect
{
    public InventoryItem Item => _item;
    private InventoryItem _item;
    private TextureRect _texture;
    private Label _label;
    
    public override void _Ready()
    {
        _texture = GetNode<TextureRect>("%TextureRect");
        _label = GetNode<Label>("%Label");
    }

    public bool Equip(InventoryItem item)
    {
        if (item.GetParent() == this)
        {
            return false;
        }

        item.GetParent()?.RemoveChild(item);
        AddChild(item);

        return true;
    }
}
