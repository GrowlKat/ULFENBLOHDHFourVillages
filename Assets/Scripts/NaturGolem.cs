using System.Collections;
using UnityEngine;

/// <summary>
/// Class of the Nahtur Golem, the main enemy in game
/// </summary>
public class NaturGolem : MonoBehaviour
{
    public EnemyStats localStats;
    public static EnemyStats stats;
    public float speed = 3;
    public float visionRadius = 3.5f;
    public float attackRadius = 2.5f;
    public float damageArea = 1f;
    private Vector3 initialPosition;
    private Rigidbody2D enemyPhysics;
    private Animator enemyAnimator;
    private Vector3 target;

    public GameObject player;
    public GameObject rock;
    public float attackCooldown;
    private bool isAttacking = false;

    void Start()
    {
        stats = localStats;
        enemyAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        target = initialPosition;
        enemyPhysics = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // If the player is not null, the distance between enemy and player will be calculated
        float distance = Vector2.Distance(
            transform.position,
            player is not null ? player.transform.position : Vector2.zero);

        // If distance is lower than the damage area of the enemy and the player attacked, the enemy decreases
        // it's health with the player attack value, plays the damage sound and get pushed back
        if (distance <= damageArea && Player.isAttacking)
        {
            SFXController.PlaySFX("EnemyDamage");
            localStats.life -= Player.stats.attack;
            enemyPhysics.AddForce(new(
                Mathf.Sign(transform.position.x - player.transform.position.x) * 300f,
                Mathf.Sign(transform.position.y - player.transform.position.y) * 300f),
                ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // If the player is not null, the distance between enemy and player will be calculated
        float distance = Vector2.Distance(
            transform.position,
            player is not null ? player.transform.position : Vector2.zero);
        Vector3 direction = (target - transform.position).normalized; // Calculates the direction

        // Gets the X and Y value for the animator and some utilities
        float xDir = Mathf.Ceil(direction.x);
        float yDir = Mathf.Ceil(direction.y);
        
        // Sets the cartesian values to the animator
        enemyAnimator.SetFloat("movX", xDir);
        enemyAnimator.SetFloat("movY", yDir);

        // Occurs if the player is below the attack radius and the vision radius
        if (distance <= attackRadius && distance < visionRadius && player is not null)
        {
            if (!isAttacking)
            {
                StartCoroutine(Attack(attackCooldown)); // Attacks if it's not currently attacking
            }
        }
        // If the distance is below vision radius and the player is not null
        else if (distance <= visionRadius && player is not null)
        {
            target = player.transform.position; // Follows the player

            // Moves the enemy to the specified direction and animates it's movement
            enemyPhysics.MovePosition(transform.position + direction * speed * Time.deltaTime);
            enemyAnimator.SetBool("moving", true);
        }
        // If player is not at the range of the enemy
        else
        {
            // Stop moving if it's position is the initial one
            target = initialPosition;
            if (transform.position != initialPosition)
            {
                enemyAnimator.SetBool("moving", true);
            }
            else if (transform.position == initialPosition && distance < visionRadius)
            {
                enemyAnimator.SetBool("moving", false);
            }
            enemyAnimator.SetBool("moving", false);
        }

        // The enemy is destroyed if it's health becomes 0 or lower, play it's death sound,
        // decreases the enemy quantity and finally check if there's no more enemies
        if (localStats.life <= 0)
        {
            SFXController.PlaySFX("EnemyDie");
            MainQuest.EnemyQuantity--;
            MainQuest.CheckRemainingEnemies();
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        // Draw some circles to represent in Gizmos the enemy ranges
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageArea);
    }

    /// <summary>
    /// Excetute an attack with some cooldown
    /// </summary>
    /// <param name="cooldown">The cooldown time to do another attack</param>
    /// <returns></returns>
    private IEnumerator Attack(float cooldown)
    {
        // Notifies the enemy is attacking
        isAttacking = true;

        // If target is not the initial position and rock prefab is not null, instantiates a rock,
        // set it's attack as enemy's attack and wait the specified cooldown to do another attack
        if (target != initialPosition && rock != null)
        {
            GameObject newRock = Instantiate(rock, transform.position, transform.rotation);
            newRock.GetComponent<Rock>().atk = localStats.attack;
            yield return new WaitForSeconds(cooldown);
        }

        // Notifies the enemy is not attacking
        isAttacking = false;
    }
}
