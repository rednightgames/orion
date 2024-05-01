namespace ResourceManager;

using Godot;

public class TextureResource : BasicResource
{
    public string Texture
    {
        get => _texturePath;
        set
        {
            if (_texturePath == null)
            {
                _texturePath = PathManager.ResolvePath(value);
            }
        }
    }
    public Texture2D Resource
    {
        get => _texture;
        set
        {
            GD.Print("aaa", _texturePath);
            if (_texture == null)
            {
                _texture = value;
            }
        }
    }
    private string _texturePath;
    private Texture2D _texture;
}
