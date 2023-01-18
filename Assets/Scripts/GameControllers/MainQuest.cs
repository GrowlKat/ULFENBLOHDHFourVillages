using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the main objective in game
/// </summary>
public class MainQuest : MonoBehaviour
{
    public Text victoryText;

    private Text victoryText2;

    private static Action endBattle;
    private static Action showVictoryText;
    private MainQuest _mainQuest;
    public static Quest mainQuest = Quest.singleton;
    public static bool victoryTransition;
    public Quest quest;

    public static int EnemyQuantity {
        get { return mainQuest.enemyQuantity; }
        set { mainQuest.enemyQuantity = value; } }

    private void Awake()
    {
        victoryText2 = victoryText.GetComponentInChildren<Text>();

        _mainQuest = GetComponent<MainQuest>();
        mainQuest.enemyQuantity = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // This lamda top the music and then plays the victory theme
        endBattle = new(() =>
        {
            StartCoroutine(MusicController.StopMusic(2f));
            StartCoroutine(PlayVictoryTheme());
        });

        // Starts the coroutine to show the victory text (This cannot be set normally in an static method)
        showVictoryText = new(() => StartCoroutine(ShowVictoryScreen()));
    }

    private void Start()
    {
        // Check if theres enemies in the scene, meaning theres an active battle
        mainQuest.currentlyInBattle = EnemyQuantity > 0;

        // When the level starts, the loads the music of this zone, checking if it needs to play the battle
        // theme or the ambience theme, checking if there's an active battle
        MusicController.PlayMusic(SceneManager.GetActiveScene().name
                + (mainQuest.currentlyInBattle ? "Battle" : "Ambience"));
    }

    private void Update()
    {
        // Updates in real time the information in the inspector of the quest, and prevents it from being diabled
        quest = mainQuest;
        _mainQuest.enabled = true;
    }

    /// <summary>
    /// Checks if there's enemies in scene, if not, ends the battle state and excecutes the end of the battle code
    /// </summary>
    public static void CheckRemainingEnemies()
    {
        if (EnemyQuantity <= 0)
        {
            mainQuest.currentlyInBattle = false;
            endBattle();
        }
    }

    /// <summary>
    /// Plays the victory theme, shows the victory text, restores the health of the player, and updates the
    /// completed zones, to finally start playing the ambience music in scene
    /// </summary>
    /// <returns></returns>
    private static IEnumerator PlayVictoryTheme()
    {
        victoryTransition = true;
        yield return new WaitForSeconds(2f);
        SFXController.PlaySFX("VictorySound");
        showVictoryText();
        Player.stats.life = 200;
        yield return new WaitForSeconds(13f);
        victoryTransition = false;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Shohtzsfen":
                mainQuest.shohtzsfenCompleted = true;
                break;

            case "Ballaghan":
                mainQuest.ballaghanCompleted = true;
                break;

            case "Ayotzor":
                mainQuest.ayotzorCompleted = true;
                break;

            case "Eilighvind":
                mainQuest.eilighvindCompleted = true;
                break;
        }
        MusicController.PlayMusic(SceneManager.GetActiveScene().name + "Ambience");
    }

    /// <summary>
    /// Show the victory screen and wait until the victory music is over
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowVictoryScreen()
    {
        for (float i = 0; i < 1; i += Time.deltaTime / 5)
        {
            victoryText.color = new(1, 1, 1, i);
            victoryText2.color = new(1, 1, 1, i);
        }
        yield return new WaitForSeconds(13f);
        for (float i = 1; i >= 0; i -= Time.deltaTime / 5)
        {
            victoryText.color = new(1, 1, 1, i);
            victoryText2.color = new(1, 1, 1, i);
        }
    }
}
