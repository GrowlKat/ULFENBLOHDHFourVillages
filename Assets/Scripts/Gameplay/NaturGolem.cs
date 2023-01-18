using System.Collections;
using UnityEngine;

/// <summary>
/// Class of the Nahtur Golem, the main enemy in game
/// </summary>
public class NaturGolem : Enemy
{
    public GameObject rock;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
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

        base.FixedUpdate();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    /// <summary>
    /// Excetute an attack with some cooldown
    /// </summary>
    /// <param name="cooldown">The cooldown time to do another attack</param>
    private IEnumerator Attack(float cooldown)
    {
        // Notifies the enemy is attacking
        isAttacking = true;

        // If target is not the initial position and rock prefab is not null, instantiates a rock,
        // set it's attack as enemy's attack and wait the specified cooldown to do another attack
        if (target != initialPosition && rock != null)
        {
            GameObject newRock = Instantiate(rock, transform.position, transform.rotation);
            newRock.GetComponent<Rock>().stats.attack = stats.attack;
            yield return new WaitForSeconds(cooldown);
        }

        // Notifies the enemy is not attacking
        isAttacking = false;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded || !MainQuest.mainQuest.currentlyInBattle) return;

        float result = Random.value;
        if (result < 0.5f) Instantiate(drops["LunarWater"], transform.position, transform.rotation);
    }
}