using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads the player in a different position based on what level is getting loaded
/// </summary>
public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerPrefab;
    public Vector3 position;

    private void Awake()
    {
        // Gets what scene is getting loaded, if it's not Main Menu, loads the player
        // on a different position of the world depending of what level is loaded
        SceneManager.activeSceneChanged += new((old, next) =>
        {
            if (next.name != "MainMenu")
            {
                var player = Instantiate(playerPrefab); // Instantiate the player with a prefab
                DontDestroy.Add(player); // Adds the player to DontDestroyOnLoad

                // Sets the player position depending of the level loaded
                player.transform.position = next.name switch
                {
                    "Shohtzsfen" => new(-5, 0, 0),
                    "Ballaghan" => new(30, 0, 0),
                    "Ayotzor" => new(-1.5f, -60, 0),
                    "Eilighvind" => new(0.5f, -67, 0),
                    _ => new()
                };

                // Activates the player gameObject
                player.SetActive(true);
            }
        });
    }
}
