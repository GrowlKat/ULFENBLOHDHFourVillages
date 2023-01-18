using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains information about the game soundtrack and a singleton to manage it
/// </summary>
public class MusicList
{
    public List<AudioSource> musicList = new();
    public Dictionary<string, AudioSource> musicDictionary = new();
    public AudioSource audioSource = new();

    public static MusicList singleton = new();
}
