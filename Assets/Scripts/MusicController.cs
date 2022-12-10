using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the music played in game
/// </summary>
public class MusicController : MonoBehaviour
{
    public List<GameObject> musicList;
    private AudioSource _audioSource;

    private static MusicList _musicList = new();
    private static AudioSource AudioSource
    {
        get { return _musicList.audioSource; }
        set { _musicList.audioSource = value; }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        List<AudioSource> _musicList = new();
        Dictionary<string, AudioSource> _musicDictionary = new();

        // Gets every object in the initial Music List and loads it into the internal list and dictionary
        foreach(var music in musicList)
        {
            _musicList.Add(music.GetComponent<AudioSource>());
            _musicDictionary.Add(music.name, music.GetComponent<AudioSource>());
        }

        // Loads the information to the singleton
        MusicList.singleton.musicList = _musicList;
        MusicList.singleton.musicDictionary = _musicDictionary;
        MusicList.singleton.audioSource = _audioSource;

        // The list loaded in singleton is set to the internal list
        MusicController._musicList = MusicList.singleton;
    }

    private void OnEnable()
    {
        // When enabled, gets the audio source attached to gameObject
        _audioSource = GetComponent<AudioSource>();

        List<AudioSource> _musicList = new();
        Dictionary<string, AudioSource> _musicDictionary = new();

        // Gets every object in the initial Music List and loads it into the internal list and dictionary
        foreach (var music in musicList)
        {
            _musicList.Add(music.GetComponent<AudioSource>());
            _musicDictionary.Add(music.name, music.GetComponent<AudioSource>());
        }

        // Loads the information to the singleton
        MusicList.singleton.musicList = _musicList;
        MusicList.singleton.musicDictionary = _musicDictionary;
        MusicList.singleton.audioSource = _audioSource;

        // The list loaded in singleton is set to the internal list
        MusicController._musicList = MusicList.singleton;
    }

    /// <summary>
    /// Get an audio clip by it's ID
    /// </summary>
    /// <param name="id">the ID of the clip</param>
    /// <returns>The audio clip with that ID</returns>
    public static AudioClip GetAudioClip(int id)
    {
        return _musicList.musicList[id].clip;
    }

    /// <summary>
    /// Get an audio clip by it's name
    /// </summary>
    /// <param name="name">the name of the clip</param>
    /// <returns>The audio clip with that name</returns>
    public static AudioClip GetAudioClip(string name)
    {
        return _musicList.musicDictionary[name].clip;
    }

    /// <summary>
    /// Plays the music ny it's ID if it's not null
    /// </summary>
    /// <param name="id">The ID of the clip</param>
    public static void PlayMusic(int id)
    {
        AudioSource.clip = GetAudioClip(id);
        if (AudioSource is not null) AudioSource.Play();
    }

    /// <summary>
    /// Plays the music ny it's name if it's not null
    /// </summary>
    /// <param name="name">The name of the clip</param>
    public static void PlayMusic(string name)
    {
        AudioSource.clip = GetAudioClip(name);
        if (AudioSource is not null && AudioSource.enabled == true) AudioSource.Play();
    }

    /// <summary>
    /// Stops the music with a fade time in seconds
    /// </summary>
    /// <param name="fadeTime">The time of the fading</param>
    /// <returns></returns>
    public static IEnumerator StopMusic(float fadeTime)
    {
        // Gets the original volume in the audio clip
        float startVolume = AudioSource.volume;

        // Decrements the volume until it's 0
        while (AudioSource.volume > 0 && AudioSource.isPlaying)
        {
            AudioSource.volume -= startVolume * (Time.deltaTime / fadeTime);
            yield return null;
        }

        // Finally stop the clip and return the Audio Source to it's original volume
        AudioSource.Stop();
        AudioSource.volume = startVolume;
    }

    private void Update()
    {
        // Some bug fixing code, to prevent the object from being randomly disabled
        if (_musicList == null)
        {
            gameObject.SetActive(true);
            AudioSource.enabled = true;
        }
    }
}