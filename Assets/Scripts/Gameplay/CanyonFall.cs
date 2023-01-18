using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CanyonFall : MonoBehaviour
{
    private TilemapCollider2D trigger;
    private Player player;
    public static Vector3 fallDirection;

    private void Start()
    {
        trigger = GetComponent<TilemapCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        Animator animator = player.GetComponent<Animator>();

        animator.SetTrigger("falling");
        player.isFalling = true;
        player.canAttack = false;
        player.canDodge = false;
        player.canMove = false;
        player.transform.position = new(
            player.transform.position.x + animator.GetFloat("idleX") + (animator.GetFloat("idleX") * 0.3f),
            player.transform.position.y + animator.GetFloat("idleY") + (animator.GetFloat("idleY") * 0.3f),
            player.transform.position.z);
        yield return new WaitForSeconds(0.8f);
        player.localStats.life = 0;
    }
}