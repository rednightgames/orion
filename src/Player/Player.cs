namespace Player;

using Godot;

public partial class Player : CharacterBody3D
{
    [Export]
    public string id;
    public Stats Stats;
    private Movement _movement;
    private ItemPickUp _itemPickUp;

    public override void _Ready()
    {
        var inventory = new Inventory(this);
        var area = GetNode<Area3D>("Area");
        var navigation = GetNode<NavigationAgent3D>("Navigation");
        var animation = GetNode<AnimationTree>("AnimationTree");
        Stats = new(id);
        _itemPickUp = new(this, inventory, area, animation, navigation);
        _movement = new(this, Stats, _itemPickUp, animation, navigation);
    }

    public override void _PhysicsProcess(double delta)
    {
        _movement.Moving(delta);   
    }
    //public void Reset()
    //{
    //    _stats.Reset();
    //}
}
