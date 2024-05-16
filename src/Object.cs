using System;
using Godot;
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
            ObjectData data = AssetManager.Load<ObjectData>(_id);
            if (data != null)
            {
                if (!Engine.IsEditorHint())
                {
                    GD.Print(data.Test);
                }
                Name = data.Name;
                _sprite.Texture = data.Resource;
            }
        }
    }
}

class ObjectData : TextureResource
{
    public string Name;
    public bool Test;
}
