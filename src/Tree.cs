using Godot;

public partial class tree : Node3D
{
    private CharacterBody3D Player;
    private Sprite3D PlayerSprite;
    private Sprite3D sprite;

    public override void _Ready()
    {
        Player = GetNode<CharacterBody3D>("../Player");
        PlayerSprite = GetNode<Sprite3D>("../Player/Sprite3D");
        sprite = GetNode<Sprite3D>("StaticBody3D/Sprite3D");
    }

    public override void _Process(double delta)
    {
        sprite.Rotation = new Vector3(PlayerSprite.Rotation.X, Player.Rotation.Y, Rotation.Z);
    }
}
