using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelGenOutput
{
	public LevelGenMap.TileType[,] Grid;
    public Dictionary<string, LevelGenMapInfo> MapInfo;

    public void AddMapInfo(LevelGenMapInfo info)
    {
        if (this.MapInfo.ContainsKey(info.Name))
        {
            foreach (var entity in info.Data)
            {
                this.MapInfo[info.Name].Data.Add(entity);
            }
        }
        else
        {
            this.MapInfo[info.Name] = info;
        }
    }

    public void AppendOutput(LevelGenOutput output)
    {
        foreach (string key in output.MapInfo.Keys)
        {
            this.AddMapInfo(output.MapInfo[key]);
        }
    }
}

public class LevelGenMapInfo
{
    public string Name;
    public IList Data;
}

public class LevelGenRoomInfo : LevelGenMapInfo
{
    public LevelGenRoomInfo(List<SimpleRect> rooms)
    {
        this.Name = "rooms";
        this.Data = rooms;
    }
}

public class LevelGenCorridorInfo : LevelGenMapInfo
{
    public LevelGenCorridorInfo(List<LevelGenMap.Coordinate> corridors)
    {
        this.Name = "corridors";
        this.Data = corridors;
    }
}
