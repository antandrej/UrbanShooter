using UnityEngine;
using UnityEngine.UI;

public class TimeElapsed : MonoBehaviour
{
    public PlayerShooting ps;
    private Text timeText;

    void OnEnable()
{
    timeText = GetComponent<Text>();
    if (ps != null && !ps.isAlive && !ps.finalTimeSet)
    {
        int minutes = Mathf.FloorToInt(ps.elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(ps.elapsedTime % 60f);
        timeText.text = $"Time Survived : {minutes:00}m {seconds:00}s";
        ps.finalTimeSet = true;
    }
}
}
