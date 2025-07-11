using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float maxHealth = 100f;
    public float currentHealth;

    public float shootCooldown = 1f;
    public float fireRange = 35f;
    public float baseInaccuracyDegrees = 2f;

    public Transform firePoint;
    public LayerMask sightLayers;

    private float shootTimer = 0f;

    private PlayerShooting isAlive;
    void Start()
    {
        isAlive = player.GetComponent<PlayerShooting>();
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

        shootTimer += Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= fireRange && shootTimer >= Random.Range(shootCooldown, shootCooldown + 3) && CanSeePlayer() && isAlive.isAlive)
        {
            ShootAtPlayer(distance);
            shootTimer = 0f;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dir = (player.position - firePoint.position).normalized;
        if (Physics.Raycast(firePoint.position, dir, out RaycastHit hit, fireRange, sightLayers))
        {
            return hit.transform == player;
        }
        return false;
    }

    void ShootAtPlayer(float distance)
    {
        Vector3 targetDirection = (player.position - firePoint.position).normalized;

        float inaccuracy = baseInaccuracyDegrees * (distance / fireRange);
        Quaternion spread = Quaternion.Euler(
            Random.Range(-inaccuracy, inaccuracy),
            Random.Range(-inaccuracy, inaccuracy),
            0f
        );
        Vector3 shotDirection = spread * targetDirection;
        Debug.DrawRay(firePoint.position, shotDirection * fireRange, Color.red, 1f);


        if (Physics.Raycast(firePoint.position, shotDirection, out RaycastHit hit, fireRange, sightLayers))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerShooting player = hit.collider.GetComponent<PlayerShooting>();
                if (player != null)
                {
                    int damage = Random.Range(35, 45);
                    player.TakeDamage(damage);
                }
            }
            else
            {
                // TODO: Impact feedback
            }
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
    GameManager gm = FindObjectOfType<GameManager>();
    if (gm != null)
    {
        gm.RegisterKill();
    }

    Destroy(this.gameObject);
}
}