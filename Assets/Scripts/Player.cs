using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The class for the player and it's control
/// </summary>
public class Player : MonoBehaviour
{
    public PlayerStats localStats;
    public static PlayerStats stats;

    public Inventory inventory;
    public int currentLunarWaters;

    public static bool isAttacking;
    public bool isFalling = false;
    public bool canMove;
    public bool canAttack;
    public bool canDodge;
    public float speed = 5f;
    public float attackCooldown;
    public GameObject attackTrigger;

    public float jumpCooldown = 0.5f;
    public float rollCooldown = 0.5f;
    public float dashCooldown = 0.5f;
    public float currentCD;
    public float timeAfterKey;

    private bool paused;
    private bool exiting;

    private Rigidbody2D body;
    private Animator playerAnim;
    private Animator transitionAnimation;
    private Camera mainCamera;

    private KeyCode prevKey;
    private KeyCode actualKey;

    private void Start()
    {
        stats = localStats;
        canMove = true;
        canAttack = true;
        playerAnim = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        transitionAnimation = GameObject.Find("Transition").GetComponent<Animator>();

        // Excecutes when the current scene is changed
        SceneManager.activeSceneChanged += (current, next) =>
        {
            // Find in scene the objects the player needs for the excecution of the game
            transitionAnimation = GameObject.Find("Transition").GetComponent<Animator>();
        };

        // Sends it's game object to DontDestroyOnLoad and activates it
        DontDestroy.Add(gameObject);
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // Determinates when the player is attacking
        isAttacking = (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E)) & canAttack;

        // If the player is attacking and the game is not paused, excutes an attack
        if (isAttacking && !paused)
        {
            StartCoroutine(Attack(attackCooldown));
        }

        if (inventory.lunarWaters.Count > 0 && Input.GetKeyDown(KeyCode.Z) && stats.life < stats.maxLife)
        {
            inventory.lunarWaters.Pop().Use();
        }

        // Pauses the game if Escape is pressed, and deactivates it if pressed again
        if (Input.GetKeyDown(KeyCode.Escape) && !exiting && !MainQuest.victoryTransition) paused = !paused;
        Time.timeScale = paused || exiting ? 0 : 1;
        UICanvas.Pause(paused || exiting); // Activates the pause menu if the game is paused

        // If the game is paused, and space is pressed and not exiting scene, it goes to the main menu
        if (paused && !exiting)
        {
            if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(ToMainMenu());
        }
        currentLunarWaters = inventory.lunarWaters.Count;
    }

    private void FixedUpdate()
    {
        // The camera follows the player
        mainCamera.transform.position = new(transform.position.x, transform.position.y, -10);

        // Sets the moving parameter in the Animator to false as default
        playerAnim.SetBool("moving", false);

        // Sets the cartesian direction to 0 as default in the Animator
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) playerAnim.SetFloat("movY", 0);
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) playerAnim.SetFloat("movX", 0);

        // Control the movement of the player, gets AWSD keys, move the player to the specified
        // direction and sets a cartesian value to the animator
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, 1f * speed * Time.deltaTime, 0);
                playerAnim.SetBool("moving", true);
                playerAnim.SetFloat("movY", 1);
                playerAnim.SetFloat("idleX", 0);
                playerAnim.SetFloat("idleY", 1);
                prevKey = actualKey;
                actualKey = KeyCode.W;
                if (Input.GetKeyDown(KeyCode.W) && prevKey == KeyCode.W && timeAfterKey < 0.1f)
                {
                    Dash(playerAnim);
                    prevKey = KeyCode.None;
                    actualKey = KeyCode.None;
                }
                timeAfterKey = 0;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, -1f * speed * Time.deltaTime, 0);
                prevKey = actualKey;
                playerAnim.SetBool("moving", true);
                playerAnim.SetFloat("movY", -1);
                playerAnim.SetFloat("idleX", 0);
                playerAnim.SetFloat("idleY", -1);
                actualKey = KeyCode.S;
                if (Input.GetKeyDown(KeyCode.S) && prevKey == KeyCode.S && timeAfterKey < 0.1f)
                {
                    Dash(playerAnim);
                    prevKey = KeyCode.None;
                    actualKey = KeyCode.None;
                }
                timeAfterKey = 0;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-1f * speed * Time.deltaTime, 0, 0);
                playerAnim.SetBool("moving", true);
                playerAnim.SetFloat("movX", -1);
                playerAnim.SetFloat("idleX", -1);
                playerAnim.SetFloat("idleY", 0);
                prevKey = actualKey;
                actualKey = KeyCode.A;
                if (Input.GetKeyDown(KeyCode.A) && prevKey == KeyCode.A && timeAfterKey < 0.1f)
                {
                    Dash(playerAnim);
                    prevKey = KeyCode.None;
                    actualKey = KeyCode.None;
                }
                timeAfterKey = 0;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(1f * speed * Time.deltaTime, 0, 0);
                playerAnim.SetBool("moving", true);
                playerAnim.SetFloat("movX", 1);
                playerAnim.SetFloat("idleX", 1);
                playerAnim.SetFloat("idleY", 0);
                prevKey = actualKey;
                actualKey = KeyCode.D;
                if (Input.GetKeyDown(KeyCode.D) && prevKey == KeyCode.D && timeAfterKey < 0.1f)
                {
                    Dash(playerAnim);
                    prevKey = KeyCode.None;
                    actualKey = KeyCode.None;
                }
                timeAfterKey = 0;
            }
        }

        // Manages the cooldowns
        timeAfterKey += timeAfterKey <= 0.1f ? Time.deltaTime : 0;
        currentCD -= currentCD > 0 ? Time.deltaTime : 0;
        canDodge = currentCD <= 0;

        // Kill the player if health is bellow 0, excecutes the death sound and reload the scene
        if (stats.life <= 0)
        {
            SFXController.PlaySFX("PlayerDie");
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Destroys the player object if it's disabled (It isn't detected by DontDestroy class)
        if (!gameObject.activeInHierarchy) Destroy(gameObject, 1);
    }

    /// <summary>
    /// Excecute an attack
    /// </summary>
    /// <param name="cooldown">The time the player shall wait to attack again</param>
    /// <returns></returns>
    private IEnumerator Attack(float cooldown)
    {
        attackTrigger.SetActive(true); // Activates the attack trigger
        canAttack = false; // Notifies the player is attacking
        playerAnim.SetBool("attacking", true); // Animates the attack
        playerAnim.SetBool("moving", false); // Stop animating the movement
        canMove = false; // The player cannot move while attacking
        SFXController.PlaySFX("Attack"); // Plays the attack sound

        yield return new WaitForSeconds(cooldown); // Wait the specified cooldown

        // Revert the previous changes once the cooldown ends
        canMove = true;
        isAttacking = false;
        playerAnim.SetBool("attacking", false);
        playerAnim.SetBool("moving", true);
        canAttack = true;
        attackTrigger.SetActive(false);
    }

    private IEnumerator Jump()
    {
        canAttack = false;
        //playerAnim.SetBool("attacking", true);
        //playerAnim.SetBool("moving", false);
        //Vector3 dodgeDirection = new Vector3(playerAnim.GetFloat("movX"), playerAnim.GetFloat("movY"), 0);
        //Vector3 increment = new Vector3();
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            transform.position += new Vector3(
                Time.deltaTime * playerAnim.GetFloat("movX"),
                Time.deltaTime * playerAnim.GetFloat("movY"),
                0);
        }
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        //playerAnim.SetBool("attacking", false);
        //playerAnim.SetBool("moving", true);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Gets the enemy who collided with the player and detects if it was an attack, if it was,
        // the player is pushed back
        GameObject currentEnemy;
        if (collision.gameObject.layer == 8)
        {
            currentEnemy = collision.gameObject;
            body.AddForce(new(
                Mathf.Sign(transform.position.x - currentEnemy.transform.position.x) * 200f,
                Mathf.Sign(transform.position.y - currentEnemy.transform.position.y) * 200f));
        }
    }

    public void Dash(Animator animator)
    {
        //If the cooldown is over, the player jumps and sets the cooldown at default
        /*if (canDodge)
        {
            for (float i = 0; i < 0.5f; i += Time.deltaTime)
            {
                transform.position += new Vector3(
                    (animator.GetFloat("idleX") * 2f) * Time.deltaTime,
                    (animator.GetFloat("idleY") * 2f) * Time.deltaTime);
            }
            currentCD = dashCooldown;
            //yield return new WaitForSeconds(dashCooldown);
        }*/
    }

    /// <summary>
    /// Loads the scene transition and loads the main menu
    /// </summary>
    /// <returns></returns>
    public IEnumerator ToMainMenu()
    {
        canMove = false; // Player cannot move
        exiting = true; // Notificates the game is being closed
        transitionAnimation.SetTrigger("SceneLoaded"); // Loads the transition animation
        yield return new WaitForSecondsRealtime(1f); // Wait 1sec
        SceneManager.LoadScene("MainMenu"); // Loads Main Menu
        paused = false; // Notifies the pause is stopped
        Time.timeScale = 1; // Returns time scale to 1, stopping pause
        DontDestroy.Remove(gameObject); // Removes the player from DontDestroyOnLoad and destroys it
    }
}
