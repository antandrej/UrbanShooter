using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Animator animator;

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
