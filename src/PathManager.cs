using System.Collections.Generic;

public class PathManager
{
    private static Dictionary<string, string> _pathMappings = new Dictionary<string, string>()
    {
        { "characters://", "res://assets/characters/" },
        { "items://", "res://assets/items/" }
    };

    public static string ResolvePath(string path)
    {
        foreach (var mapping in _pathMappings)
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
