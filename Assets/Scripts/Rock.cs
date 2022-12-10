using UnityEngine;

/// <summary>
/// A rock used by Nahtur Golems to attack the player
/// </summary>
public class Rock : MonoBehaviour
{
    public float speed;
    public int atk;

    private GameObject player;
    private Rigidbody2D bodyPhysics;
    private Vector3 target, dir;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bodyPhysics = gameObject.GetComponent<Rigidbody2D>();

        // If player is not null, set it as it's target
        if (player != null)
        {
            target = player.transform.position;
            dir = (target - transform.position).normalized;
        }
    }

    private void FixedUpdate()
    {
        // Moves the rock to it's target
        if (target != Vector3.zero)
        {
            bodyPhysics.MovePosition(transform.position + (dir * speed) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If rock collides with an attack of the player, destroys it and play the clash sound
        if (collision.transform.CompareTag("Player Attack"))
        {
            SFXController.PlaySFX("PlayerClash");
            Destroy(gameObject);
        }

        // If rock collides with everything except an enemy and an enemy attack, destroys it
        if (!collision.transform.CompareTag("Enemy") && !collision.transform.CompareTag("Enemy Attack"))
        {
            Destroy(gameObject);
            // If rock collides with the player, destroys it and play the clash sound, also decreases
            // player's health with the rock's attack
            if (collision.transform.CompareTag("Player"))
            {
                SFXController.PlaySFX("PlayerDamage");
                Player.stats.life -= atk;
            }
        }
    }

    private void OnBecameInvisible()
    {
        // Destroys the object when is no more visible in any camera
        Destroy(gameObject);
    }
}
