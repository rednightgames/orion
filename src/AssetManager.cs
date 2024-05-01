using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Newtonsoft.Json;
using ResourceManager;

public partial class AssetManager : Node
{
    private Dictionary<string, string> _directoryTypeMap = new Dictionary<string, string>
    {
        { "res://data/items/", nameof(ItemData) }
    };
    private Dictionary<string, Func<string, object>> _dataHandlers = new Dictionary<
        string,
        Func<string, object>
    >()
    {
        { nameof(ItemData), (jsonString) => JsonConvert.DeserializeObject<ItemData>(jsonString) }
    };
    private Dictionary<string, object> _cache = new Dictionary<string, object>() { };
    private Dictionary<string, object> _loadedCache = new Dictionary<string, object>() { };

    public override void _Ready()
    {
        foreach (var entry in _directoryTypeMap)
        {
            ListDirectoriesAndFiles(entry.Key, entry.Value);
        }
    }

    public T Load<T>(string id)
        where T : TextureResource
    {
        T loadableObject = (T)_cache[id];
        _cache.Remove(id);
        loadableObject.Resource = ResourceLoader.Load<Texture2D>(loadableObject.Texture);
        _loadedCache.Add(id, loadableObject);
        GD.Print("loadableObject.Texture  ", loadableObject.Texture);
        GD.Print("loadableObject.Resource ", loadableObject.Resource);
        return loadableObject;
    }

    private void ListDirectoriesAndFiles(string directoryPath, string dataType)
    {
        var dir = DirAccess.Open(directoryPath);
        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                if (dir.CurrentIsDir())
                {
                    ListDirectoriesAndFiles(Path.Combine(directoryPath, fileName), dataType);
                }
                else
                {
                    LoadAndProcessJson(Path.Combine(directoryPath, fileName), dataType);
                }
                fileName = dir.GetNext();
            }
        }
    }

    private void LoadAndProcessJson(string filePath, string dataType)
    {
        try
        {
            GD.Print("Loading ", filePath);

            string jsonString = Godot.FileAccess.GetFileAsString(filePath);

            if (_dataHandlers.ContainsKey(dataType))
            {
                object info = _dataHandlers[dataType].Invoke(jsonString);

                _cache.Add((info as BasicResource).Id, info);
            }
            else
            {
                GD.Print("Unknown data type");
            }
        }
        catch (Exception e)
        {
            GD.Print("Error loading or processing JSON: " + e.Message);
        }
    }
}
