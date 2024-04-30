using System;
using System.Collections.Generic;
using System.IO;
using Godot;

public partial class AssetManager : Node
{
    private Dictionary<string, string> _directoryTypeMap = new Dictionary<string, string>
    {
        { "res://data/items/", nameof(ItemData) }
    };

    //private Dictionary<string, Func<Dictionary<object, object>, object>> _dataHandlers = new Dictionary<string, Func<Dictionary<object, object>, object>>()
    //{
    //    { nameof(ItemData), (jsonData) => new ItemData(jsonData) }
    //};

    public override void _Ready()
    {
        foreach (var entry in _directoryTypeMap)
        {
            ListDirectoriesAndFiles(entry.Key, entry.Value);
        }
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

            Godot.Collections.Dictionary jsonData = ResourceLoader
                .Load<Json>(filePath)
                .Data.As<Godot.Collections.Dictionary>();

            if (_directoryTypeMap.ContainsValue(dataType))
            {
                if (dataType == nameof(ItemData))
                {
                    GD.Print(new ItemData(jsonData).Texture);
                }
            }
            else
            {
                GD.Print("Unknown data type");
            }

            GD.Print(jsonData);
        }
        catch (Exception e)
        {
            GD.Print("Error loading or processing JSON: " + e.Message);
        }
    }
}
