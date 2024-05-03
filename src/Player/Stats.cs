namespace Player;

using System;
using System.Collections.Generic;
using System.Reflection;
using ResourceManager;

public class Stats
{
    public float speed;
    public int hp;
    private Dictionary<string, object> _initialPropertyValues = new Dictionary<string, object> { };

    public Stats(string id)
    {
        PlayerStats data = AssetManager.Load<PlayerStats>(id);
        foreach (PropertyInfo prop in typeof(PlayerStats).GetProperties())
        {
            _initialPropertyValues[prop.Name] = prop.GetValue(data);
        }

        speed = data.speed;
        hp = data.hp;
    }

    public void Reset()
    {
        hp = (int)_initialPropertyValues[nameof(hp)];
        speed = (float)_initialPropertyValues[nameof(speed)];
    }
}

public class PlayerStats : BasicResource
{
    public float speed;
    public int hp;
}
