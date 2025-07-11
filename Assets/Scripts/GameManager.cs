using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Animator animator;

    public int killCount = 0;
    public Text killCounterText;

    public void RegisterKill()
    {
        killCount++;
        if (killCounterText != null)
            killCounterText.text = $"Kills : {killCount}";
    }

    public void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
        animator.SetTrigger("FadeIn");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
