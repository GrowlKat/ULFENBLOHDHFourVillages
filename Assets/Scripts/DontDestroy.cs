using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the objects to be sent to the DontDestroyOnLoad method,
/// with no duplication and with the posibility to remove them
/// </summary>
public class DontDestroy : MonoBehaviour
{
    [SerializeField] List<GameObject> _gameObjects;

    public static List<GameObject> gameObjects;

    public void Awake()
    {
        gameObjects = _gameObjects;

        // Adds every object in the start of the script to the DontDestroyOnLoad, cheching they're not duplicated
        foreach (var g in gameObjects) Add(g, true);
    }

    /// <summary>
    /// Adds the GameObject to the List
    /// </summary>
    /// <param name="g">The GameObject to add</param>
    public static void Add(GameObject g)
    {
        GameObject[] objects = null;

        // Gets every object with the same tag of the previous objects while they're not on the Main Menu
        if (SceneManager.GetActiveScene().name != "MainMenu") objects = GameObject.FindGameObjectsWithTag(g.tag);

        // If more than 1 objects were found, destroys every object except the first
        if (objects.Length > 1 && g is not null) for (int i = 1; i < objects.Length; i++) Destroy(objects[i]);
        // If only one object is found, send it to the DontDestroyOnLoad
        else if (objects.Length == 1) DontDestroyOnLoad(objects[0]);
    }

    /// <summary>
    /// Adds the GameObject to the List and set if the object can go to MainMenu
    /// </summary>
    /// <param name="g">The GameObject to add</param>
    /// <param name="admitMainMenu">The object is going to MainMenu?</param>
    public static void Add(GameObject g, bool admitMainMenu)
    {
        GameObject[] objects = null;

        // Gets every object with the same tag of the previous objects, first checks if it goes to MainMenu
        if (admitMainMenu) objects = GameObject.FindGameObjectsWithTag(g.tag);
        else if (SceneManager.GetActiveScene().name != "MainMenu") objects = GameObject.FindGameObjectsWithTag(g.tag);
        
        // If more than 1 objects were found, destroys every object except the first
        if (objects.Length > 1 && g is not null && objects is not null)
            for (int i = 1; i < objects.Length; i++) Destroy(objects[i]);
        // If only one object is found, send it to the DontDestroyOnLoad
        else if (objects.Length == 1) DontDestroyOnLoad(objects[0]);
    }

    /// <summary>
    /// Removes the object of the DontDestroy List and Destroys it from scene
    /// </summary>
    /// <param name="g">The object to remove</param>
    public static void Remove(GameObject g)
    {
        Destroy(g);
        gameObjects.Remove(g);
    }
}
