using UnityEngine;
using UnityEngine.UI;

public class LunarWaterIndicator : MonoBehaviour
{
    private Text indicator;
    private Player player;

    private void Start()
    {
        indicator = GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        indicator.text = player.inventory.lunarWaters.Count.ToString();
        indicator.color = indicator.text == "0" ? Color.red :
            indicator.text == player.inventory.maxLunarWaters.ToString() ? Color.green : Color.white;
    }
}
