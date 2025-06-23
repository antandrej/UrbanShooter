using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    public PlayerShooting playerShooting;
    public Text healthText;

    void Update()
    {
        if (playerShooting != null)
        {
            float x = playerShooting.currentHealth / playerShooting.maxHealth;
            Color color;

            if (x < 0.5f)
                color = Color.Lerp(Color.red, Color.yellow, x * 2f);
            else
                color = Color.Lerp(Color.yellow, Color.green, (x - 0.5f) * 2f);

            healthText.color = color;
            healthText.text = Mathf.RoundToInt(playerShooting.currentHealth) + "";
        }
    }
}
