namespace ResourceManager;

using System.Collections.Generic;

public class PathManager
{
    private static Dictionary<string, string> _texturePathsMappings = new Dictionary<
        string,
        string
    >()
    {
        { "characters://", "res://assets/characters/" },
        { "items://", "res://assets/items/" },
        { "objects://", "res://assets/objects/" }
    };

    public static string ResolveTexturePath(string path)
    {
        foreach (var mapping in _texturePathsMappings)
        {
            if (path.StartsWith(mapping.Key))
            {
                string itemName = path.Replace(mapping.Key, "");
                return mapping.Value + itemName;
            }
        }
        return path;
    }
}
