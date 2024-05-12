using System;
using Godot;

public partial class Inventory : Node
{
    private const int InventorySize = 3;

    private int activeSlotIndex = 0;

    public InventoryItem[] _items = new InventoryItem[InventorySize];
    private Player _player;

    public Inventory(Node parent)
    {
        parent.AddChild(this);
    }

    public override void _Ready()
    {
        _player = GetParent<Player>();
    }

    public void SetActiveSlot(int index)
    {
        if (index >= 0 && index < InventorySize)
        {
            activeSlotIndex = index;
            ApplyActiveItemEffects();
        }
    }

    public void AddItem(InventoryItem item, Action Delete)
    {
        for (int i = 0; i < InventorySize; i++)
        {
            if (_items[i] == null)
            {
                _items[i] = item;
                Delete();
                break;
            }
        }
    }

    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < InventorySize)
        {
            _items[slotIndex] = null;
        }
    }

    private void ApplyActiveItemEffects()
    {
        _player.ResetStats();

        _items[activeSlotIndex]?.ApplyEffect(_player);
    }
}

public abstract partial class InventoryItem : Node3D
{
    public new string Name;
    public string Description;

    public virtual void ApplyEffect(Player player) { }
}
