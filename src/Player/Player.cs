namespace Player;

using Godot;

public partial class Player : CharacterBody3D
{
    [Export]
    public string id;
    private Inventory _inventory;
    private Stats _stats;
    private Movement _movement;
    private ItemPickUp _itemPickUp;

    public override void _Ready()
    {
        _stats = new(id);
        _itemPickUp = new(this, _inventory, GetNode<NavigationAgent3D>("Navigation"));
        _movement = new(this, _stats, _itemPickUp, GetNode<AnimationTree>("AnimationTree"));
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
