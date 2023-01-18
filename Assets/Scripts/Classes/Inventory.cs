using System;
using System.Collections.Generic;

[Serializable]
public class Inventory
{
    public Stack<LunarWater> lunarWaters = new();

    public int maxLunarWaters;

    public void AddLunarWater()
    {
        if (lunarWaters.Count < maxLunarWaters) lunarWaters.Push(new(40));
    }
}