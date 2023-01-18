using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemType itemType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            switch (itemType)
            {
                case ItemType.LunarWater:
                    if (player.inventory.lunarWaters.Count < player.inventory.maxLunarWaters) Destroy(gameObject);
                    player.inventory.AddLunarWater();
                    break;
            }
        }
    }

    public enum ItemType
    {
        None,
        LunarWater
    }
}