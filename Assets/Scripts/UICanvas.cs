using UnityEngine;

/// <summary>
/// Controller for UI elements, such as Pause Menu, it prevents some bugs
/// </summary>
public class UICanvas : MonoBehaviour
{
    [SerializeField] GameObject pause;
    private static GameObject _pause;

    private void Awake()
    {
        // Adds pause to DontDestroyOnLoad
        DontDestroy.Add(gameObject);
        _pause = pause;
    }

    /// <summary>
    /// Enables or disables Pause Menu
    /// </summary>
    /// <param name="paused">Will the Pause Menu be enabled?</param>
    public static void Pause(bool paused)
    {
        _pause.SetActive(paused);
    }
}
