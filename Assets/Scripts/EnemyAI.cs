using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
