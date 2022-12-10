using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the sound effects in game
/// </summary>
public class SFXController : MonoBehaviour
{
    public List<GameObject> sfxList;

    private static List<AudioSource> _sfxList;
    private static Dictionary<string, AudioSource> _sfxDictionary;

    private static AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        _sfxList = new();
        _sfxDictionary = new();

        // Gets every object in the initial SFX List and loads it into the internal list and dictionary
        foreach (var music in sfxList)
        {
            AudioSource source = music.GetComponent<AudioSource>();
            source.enabled = true;
            _sfxList.Add(source);
            _sfxDictionary.Add(music.name, source);
        }
    }

    /// <summary>
    /// Get an audio clip by it's ID
    /// </summary>
    /// <param name="id">the ID of the clip</param>
    /// <returns>The audio clip with that ID</returns>
    public static AudioClip GetAudioClip(int id)
    {
        return _sfxList[id].clip;
    }

    /// <summary>
    /// Get an audio clip by it's name
    /// </summary>
    /// <param name="name">the name of the clip</param>
    /// <returns>The audio clip with that name</returns>
    public static AudioClip GetAudioClip(string name)
    {
        return _sfxDictionary[name].clip;
    }

    /// <summary>
    /// Plays the SFX ny it's ID
    /// </summary>
    /// <param name="id">The ID of the clip</param>
    public static void PlaySFX(int id)
    {
        audioSource.clip = GetAudioClip(id);
        audioSource.PlayOneShot(audioSource.clip);
    }

    /// <summary>
    /// Plays the SFX ny it's name
    /// </summary>
    /// <param name="name">The name of the clip</param>
    public static void PlaySFX(string name)
    {
        audioSource.clip = GetAudioClip(name);
        audioSource.PlayOneShot(audioSource.clip);
    }
}