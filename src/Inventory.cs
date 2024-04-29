using Godot;

public partial class Inventory : Node
{
    private const int InventorySize = 10;

    private int activeSlotIndex = 0;

    private Item[] _items = new Item[InventorySize];
    private Player _player;

    public Inventory(Node parent)
    {
        parent.AddChild(this);
    }

    public override void _Ready()
    {
        _player = GetParent<Player>();
        AddItem(new SpeedItem("aa", "aa", 80));
        AddItem(new SpeedItem("bb", "bb", -80));
    }

    public void SetActiveSlot(int index)
    {
        if (index >= 0 && index < InventorySize)
        {
            activeSlotIndex = index;
            ApplyActiveItemEffects();
        }
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < InventorySize; i++)
        {
            if (_items[i] == null)
            {
                _items[i] = item;
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

public abstract partial class Item : Node3D
{
    public new string Name;
    public string Description;

    public virtual void ApplyEffect(Player player) { }
}

public partial class SpeedItem : Item
{
    private float _speedBoost;

    public SpeedItem(string name, string description, float speedBoost)
    {
        Name = name;
        Description = description;
        _speedBoost = speedBoost;
    }

    public override void ApplyEffect(Player player)
    {
        player.speed += _speedBoost;
    }
}
