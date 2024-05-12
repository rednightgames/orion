using Godot;

public partial class Player : CharacterBody3D
{
    private Node3D _targetItem;
    private bool _isMovingToItem = false;
    private NavigationAgent3D _navigation;

    private void OnItemAreaEntered(Area3D area)
    {
        if (area is Node3D item)
        {
            if (item.IsInGroup("item"))
            {
                if (
                    _targetItem == null
                    || GlobalTransform.Origin.DistanceTo(item.GlobalTransform.Origin)
                        < GlobalTransform.Origin.DistanceTo(_targetItem.GlobalTransform.Origin)
                )
                {
                    _targetItem = item;
                }
            }
        }
    }

    private void OnItemAreaExited(Area3D area)
    {
        if (area is Node3D item && item == _targetItem)
        {
            _targetItem = null;
        }
    }
}
