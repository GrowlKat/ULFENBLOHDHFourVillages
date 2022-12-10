using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Control the trigger objects to load new scenes and their scene management
/// </summary>
public class SceneEntering : MonoBehaviour
{
    public string target;
    public GameObject player;
    public Vector3 destiny;
    public Village village;
    public Village destinyVillage;

    private Animator transitionAnimation;
    private Collider2D loadCollider;

    private void Awake()
    {
        // Loads the total enemies in scene, if the zone is completed, destroy the enemies
        SceneManager.sceneLoaded += (next, mode) =>
        {
            MainQuest.EnemyQuantity = GameObject.FindGameObjectsWithTag("Enemy").Length;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Shohtzsfen":
                    if (MainQuest.mainQuest.shohtzsfenCompleted)
                    {
                        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            Destroy(enemies[i]);
                        }
                        MainQuest.EnemyQuantity = 0;
                    }
                    break;

                case "Ballaghan":
                    if (MainQuest.mainQuest.ballaghanCompleted)
                    {
                        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            Destroy(enemies[i]);
                        }
                        MainQuest.EnemyQuantity = 0;
                    }
                    break;

                case "Ayotzor":
                    if (MainQuest.mainQuest.ayotzorCompleted)
                    {
                        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            Destroy(enemies[i]);
                        }
                        MainQuest.EnemyQuantity = 0;
                    }
                    break;

                case "Eilighvind":
                    if (MainQuest.mainQuest.eilighvindCompleted)
                    {
                        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                        for (int i = 0; i < enemies.Length; i++)
                        {
                            Destroy(enemies[i]);
                        }
                        MainQuest.EnemyQuantity = 0;
                    }
                    break;
            }
        };
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transitionAnimation = GameObject.FindGameObjectWithTag("Transition").GetComponent<Animator>();
        village = Enum.Parse<Village>(SceneManager.GetActiveScene().name);
        loadCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Gets the active scene and the destiny scene to determine if the zone is unlocked, and disables
        // the transition collider if there's a victory transition
        switch (village)
        {
            case Village.Shohtzsfen:
                if (MainQuest.mainQuest.shohtzsfenCompleted
                    && destinyVillage == Village.Ballaghan
                    && !MainQuest.victoryTransition)
                    loadCollider.isTrigger = true;

                if (MainQuest.mainQuest.ballaghanCompleted
                    && destinyVillage == Village.Ayotzor
                    && !MainQuest.victoryTransition)
                    loadCollider.isTrigger = true;
                break;

            case Village.Ballaghan:
                loadCollider.isTrigger = !MainQuest.victoryTransition;
                break;

            case Village.Ayotzor:
                if (destinyVillage == Village.Shohtzsfen) loadCollider.isTrigger = !MainQuest.victoryTransition;

                if (MainQuest.mainQuest.ayotzorCompleted
                    && destinyVillage == Village.Eilighvind
                    && !MainQuest.victoryTransition)
                    loadCollider.isTrigger = true;
                break;

            case Village.Eilighvind:
                loadCollider.isTrigger = !MainQuest.victoryTransition;
                break;
        }

        // To load houses while there's no battle in scene
        if (destinyVillage == Village.None && !MainQuest.victoryTransition)
            loadCollider.isTrigger = !MainQuest.mainQuest.currentlyInBattle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Loads the next scene
        if (collision.gameObject == player)
        {
            StartCoroutine(LoadScene());
        }
    }

    /// <summary>
    /// Coroutine to load the next scene and load the scene transition
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadScene()
    {
        player.GetComponent<Player>().canMove = false;
        transitionAnimation.SetTrigger("SceneLoaded");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(target);
        player.transform.position = destiny;
        player.GetComponent<Player>().canMove = true;
    }

    /// <summary>
    /// Internal enum to represent all the zones in game
    /// </summary>
    public enum Village
    {
        None,
        Shohtzsfen,
        Ballaghan,
        Ayotzor,
        Eilighvind
    }
}
