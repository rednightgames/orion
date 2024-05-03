namespace ResourceManager;

using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Newtonsoft.Json;

public static class AssetManager
{
    private static readonly Dictionary<string, string> _directoryTypeMap = new Dictionary<
        string,
        string
    >
    {
        { "res://data/items/", "items" },
        { "res://data/characters/", "characters" }
    };

    private static readonly Dictionary<string, Func<string, object>> _dataHandlers = new Dictionary<
        string,
        Func<string, object>
    >
    {
        { "items", JsonConvert.DeserializeObject<TextureResource> },
        { "characters", JsonConvert.DeserializeObject<Player.PlayerStats> }
    };

    private static readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
    private static readonly Dictionary<string, object> _loadedCache =
        new Dictionary<string, object>();

    static AssetManager()
    {
        foreach (var entry in _directoryTypeMap)
        {
            ListDirectoriesAndFiles(entry.Key, entry.Value);
        }
    }

    public static T Load<T>(string id)
        where T : BasicResource
    {
        if (_loadedCache.TryGetValue(id, out var loadedObject))
        {
            return (T)loadedObject;
        }
        else if (_cache.TryGetValue(id, out var loadableObject))
        {
            if (
                loadableObject is TextureResource textureObject
                && typeof(T) == typeof(TextureResource)
            )
            {
                textureObject.Resource = ResourceLoader.Load<Texture2D>(textureObject.Texture);
                _loadedCache.Add(id, textureObject);
                _cache.Remove(id);
                return (T)(object)textureObject;
            }
            else
            {
                GD.Print("Cached object is not of type T or not a TextureResource");
                return null;
            }
        }
        else
        {
            GD.Print("Object with id not found in cache");
            return null;
        }
    }

    private static void ListDirectoriesAndFiles(string directoryPath, string dataType)
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

    private static void LoadAndProcessJson(string filePath, string dataType)
    {
        try
        {
            GD.Print("Loading ", filePath);

            string jsonString = Godot.FileAccess.GetFileAsString(filePath);

            if (_dataHandlers.TryGetValue(dataType, out var handler))
            {
                object info = handler(jsonString);

                if (info is BasicResource basicResource)
                {
                    if (info is TextureResource textureResource)
                    {
                        if (!_loadedCache.ContainsKey(basicResource.Id))
                        {
                            _cache.Add(basicResource.Id, info);
                        }
                        else
                        {
                            GD.Print("Resource with id ", basicResource.Id, " already loaded.");
                            return;
                        }
                    }
                    else
                    {
                        _loadedCache.Add(basicResource.Id, info);
                    }
                }
                else
                {
                    GD.Print("Loaded object is not a BasicResource");
                }
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
