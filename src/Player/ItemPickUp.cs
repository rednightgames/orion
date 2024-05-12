namespace Player;

using System;
using System.Linq;
using Godot;

public class ItemPickUp
{
    public bool MovingToItem = false;
    public bool HasTargetItem => _targetItem != null;
    private static float MinimalItemCollectDistance = 0.1f;
    private Player _player;
    private Inventory _inventory;
    private Area3D _area;
    private Node3D _targetItem;
    private AnimationTree _anim;
    private AnimationNodeStateMachinePlayback _state;
    private NavigationAgent3D _navigation;

    public ItemPickUp(Player player, Inventory inventory, Area3D area, AnimationTree anim, NavigationAgent3D navigation)
    {
        _player = player;
        _inventory = inventory;
        _area = area;
        _anim = anim;
        _state = (AnimationNodeStateMachinePlayback)anim.Get("parameters/playback");
        _navigation = navigation;
        _area.AreaEntered += OnItemAreaEntered;
        _area.AreaExited += OnItemAreaExited;
    }

    public void Moving(double delta)
    {
        if (!MovingToItem)
        {
            if (!HasTargetItem)
            {
                MovingToItem = false;
                return;
            }
            if (
                _player.GlobalTransform.Origin.DistanceTo(_targetItem.GlobalTransform.Origin)
                < MinimalItemCollectDistance
            )
            {
                PickUp();
                MovingToItem = false;
                return;
            }
            var direction = _navigation.GetNextPathPosition() - _player.GlobalPosition;
            direction = direction.Normalized();

            _player.Velocity = new Vector3(direction.X, 0, direction.Z) * _player.Stats.speed * (float)delta;

            _state.Travel("walk");
            _anim.Set(
                "parameters/idle/blend_position",
                new Vector3(direction.X, direction.Z, direction.Y)
            );
            _anim.Set(
                "parameters/walk/blend_position",
                new Vector3(direction.X, direction.Z, direction.Y)
            );

            _player.MoveAndSlide();
        }
    }

    public void UpdateNavigation()
    {
        _navigation.TargetPosition = NavigationServer3D.MapGetClosestPoint(
            _player.GetWorld3D().NavigationMap,
            _targetItem.GlobalPosition
        );
        MovingToItem = true;
    }

    private void PickUp()
    {
        _inventory.AddItem((InventoryItem)_targetItem, () => {
            _targetItem.QueueFree();
            _targetItem = _area.GetOverlappingAreas().Last()?.GetParent<Node3D>();
        });
    }

    internal void OnItemAreaEntered(Area3D area)
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

    internal void OnItemAreaExited(Area3D area)
    {
        if (area is Node3D item && item == _targetItem)
        {
            _targetItem = null;
        }
    }
}
