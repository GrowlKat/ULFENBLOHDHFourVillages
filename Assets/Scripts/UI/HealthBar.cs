using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the Health Bar in the UI
/// </summary>
public class HealthBar : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();

        //If the player and it's stats is not null, sets the value of the bar with the health of the player
        if (Player.stats is not null) slider.value = Player.stats.life;
    }

    private void Update()
    {
        //If the player and it's stats is not null, sets the value of the bar with the health of the player
        if (Player.stats is not null) slider.value = Player.stats.life;
    }
}
