namespace Player;

using Godot;

public class ItemPickUp
{
    public bool _isMovingToItem = false;
    private static float MinimalItemCollectDistance = 0.1f;
    private Player _player;
    private Inventory _inventory;
    private Node3D _targetItem;
    private NavigationAgent3D _navigation;

    public ItemPickUp(Player player, Inventory inventory, NavigationAgent3D navigation)
    {
        _player = player;
        _inventory = inventory;
        _navigation = navigation;
    }

    public void PickUp()
    {
        _inventory.AddItem((InventoryItem)_targetItem, _targetItem.QueueFree);
    }

    internal void OnItemAreaEntered(Rid rid, Area3D area, int shape_index, int local_index)
    {
        if (area is Node3D item)
        {
            if (item.IsInGroup("item"))
            {
                if (
                    _targetItem == null
                    || _player.GlobalTransform.Origin.DistanceTo(item.GlobalTransform.Origin)
                        < _player.GlobalTransform.Origin.DistanceTo(
                            _targetItem.GlobalTransform.Origin
                        )
                )
                {
                    _targetItem = item.GetParent<Node3D>();
                }
            }
        }
    }

    internal void OnItemAreaExited(Rid rid, Area3D area, int shape_index, int local_index)
    {
        if (area is Node3D item && item == _targetItem)
        {
            _targetItem = null;
        }
    }
}
