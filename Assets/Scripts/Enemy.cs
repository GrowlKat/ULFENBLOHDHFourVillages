using UnityEngine;

/// <summary>
/// Base class of every enemy
/// </summary>
[System.Obsolete]
class Enemy : MonoBehaviour
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
    private GameObject player;
    //private bool canMove = true;
    //private bool beingAttacked = false;
    //private float moveCooldown = 0.3f;
    private Vector3 target;

    public GameObject rock;
    public float attackCooldown;
    //private bool isAttacking = false;

    void Start()
    {
        stats = localStats;
        enemyAnimator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        target = initialPosition;
        enemyPhysics = GetComponent<Rigidbody2D>();
    }
}
