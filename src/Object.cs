using Godot;
using System;
using ResourceManager;

[Tool]
public partial class Object : Node3D
{
	[Export]
    public string Id
    {
        get { return _id; }
        set
        {
            _id = value;
            _sprite.Texture = null;
            PostInit();
        }
    }
	private string _id = "";
    public event Action OnInit;
    private Sprite3D _sprite;

	public override void _Ready()
    {
        _sprite = GetNode<Sprite3D>("Sprite");
        OnInit?.Invoke();
        PostInit();
    }
	protected void PostInit()
    {
        if (_id != null)
        {
            TextureResource data = AssetManager.Load<TextureResource>(_id);
            if (data != null)
            {
                Name = data.Name;
                //Description = data.Description;
                _sprite.Texture = data.Resource;
            }
        }
    }
}
