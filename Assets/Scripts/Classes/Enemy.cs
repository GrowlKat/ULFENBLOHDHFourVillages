using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class of every enemy
/// </summary>
public class Enemy : MonoBehaviour
{
    public EnemyStats stats;
    public DodgingLayer dodgeLayer;
    public float speed = 3;
    public float visionRadius = 3.5f;
    public float attackRadius = 2.5f;
    public float damageArea = 1f;
    public List<GameObject> dropObjects;
    public float attackCooldown;
    public bool canMove = true;
    public bool beingAttacked = false;
    protected Vector3 target;
    protected Dictionary<string, GameObject> drops = new();
    protected Vector3 initialPosition;
    protected Rigidbody2D enemyPhysics;
    protected Collider2D enemyCollider;
    protected Animator enemyAnimator;
    protected GameObject player;
    protected bool isAttacking = false; 

    protected virtual void Start()
    {
        TryGetComponent(out enemyAnimator);
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        target = initialPosition;
        enemyPhysics = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        foreach (var o in dropObjects) drops.Add(o.name, o);
    }

    protected virtual void Update()
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
            stats.life -= Player.stats.attack;
            enemyPhysics.AddForce(new(
                Mathf.Sign(transform.position.x - player.transform.position.x) * 300f,
                Mathf.Sign(transform.position.y - player.transform.position.y) * 300f),
                ForceMode2D.Impulse);
        }
    }

    protected virtual void FixedUpdate()
    {
        // The enemy is destroyed if it's health becomes 0 or lower, play it's death sound,
        // decreases the enemy quantity and finally check if there's no more enemies
        if (stats.life <= 0)
        {
            SFXController.PlaySFX("EnemyDie");
            MainQuest.EnemyQuantity--;
            MainQuest.CheckRemainingEnemies();
            Destroy(gameObject);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        // Draw some circles to represent in Gizmos the enemy ranges
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageArea);
    }
}
