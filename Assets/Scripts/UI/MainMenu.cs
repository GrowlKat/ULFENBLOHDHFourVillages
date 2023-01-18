using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controler for the Main Menu actions
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject credits;
    public GameObject story;

    private Animator transitionAnimation;

    private void Start()
    {
        transitionAnimation = GameObject.Find("Transition").GetComponent<Animator>();

        mainMenu.SetActive(true);
        credits.SetActive(false);
        story.SetActive(false);
        MusicController.PlayMusic("MainMenu");
    }

    /// <summary>
    /// Loads the specified scene
    /// </summary>
    /// <param name="target">The screen to load</param>
    public void LoadScreen(GameObject target)
    {
        GameObject org = null;

        // Detect where this method was called
        if (target.Equals(credits)) org = mainMenu;
        if (target.Equals(mainMenu)) org = credits;
        if (target.Equals(story)) org = mainMenu;

        // Disable the origin and enables the target
        target.SetActive(true);
        org.SetActive(false);
    }

    /// <summary>
    /// Load the game specifing the scene to load
    /// </summary>
    /// <param name="name">The name of the scene</param>
    public void LoadScene(string name)
    {
        StartCoroutine(LoadGame(name));
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Exit() => Application.Quit();

    /// <summary>
    /// Enable the transition animation, stops the music and load the specified scene
    /// </summary>
    /// <param name="name">The scene to load</param>
    /// <returns></returns>
    private IEnumerator LoadGame(string name)
    {
        transitionAnimation.SetTrigger("SceneLoaded");
        StartCoroutine(MusicController.StopMusic(1f));
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }
}
