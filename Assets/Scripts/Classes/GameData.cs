using System;

[Serializable]
public class GameData
{
    
}

/// <summary>
/// Representation of every element in game
/// </summary>
public enum Element
{
    None = 0,
    Fire = 1,
    Water = 2,
    Wind = 3,
    Earth = 4,
    Light = 5,
    Darkness = 6,
    Nahtur = 7,
    Ice = 8,
    Thunder = 9
}

public enum DodgingLayer
{
    Up = 0,
    Down = 1,
    Both = 2
}