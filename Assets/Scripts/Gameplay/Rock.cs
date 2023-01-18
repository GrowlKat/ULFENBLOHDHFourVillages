using UnityEngine;

/// <summary>
/// A rock used by Nahtur Golems to attack the player
/// </summary>
public class Rock : Enemy
{
    private Vector3 dir;

    protected override void Start()
    {
        base.Start();

        // If player is not null, set it as it's target
        if (player != null)
        {
            target = player.transform.position;
            dir = (target - transform.position).normalized;
        }
    }

    private new void FixedUpdate()
    {
        gameObject.layer = dodgeLayer switch
        {
            DodgingLayer.Up => 11,
            DodgingLayer.Down => 12,
            _ => 0
        };

        // Moves the rock to it's target
        if (target != Vector3.zero)
        {
            enemyPhysics.MovePosition(transform.position + (dir * speed) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(gameObject.layer);

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
                Player.stats.life -= stats.attack;
            }
        }
    }

    private void OnBecameInvisible()
    {
        // Destroys the object when is no more visible in any camera
        Destroy(gameObject);
    }
}
