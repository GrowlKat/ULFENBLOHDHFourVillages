using UnityEngine;

/// <summary>
/// Controller of the minimap in game
/// </summary>
public class Minimap : MonoBehaviour
{
    [SerializeField] Camera minimapCamera;
    [SerializeField] GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Follows the minimap camera to the player's position, if the player is not null
        minimapCamera.transform.position = player is not null ?
            new(
                player.transform.position.x,
                player.transform.position.y,
                -10)
            : new(0, 0, -10);
    }
}
