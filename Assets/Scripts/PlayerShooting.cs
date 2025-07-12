using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Transform firePoint;
    public float shootRange = 100f;
    public LayerMask shootableLayers;

    public float maxAmmo;
    public float currentAmmo;
    public float reloadTime;
    public Text ammoText;

    public bool isAlive;

    public float elapsedTime = 0f;
    public bool finalTimeSet = false;

    public float baseInaccuracy = 2f;
    public float movementInaccuracy = 4f;
    public float aimInaccuracy = 1f;

    void Start()
    {
        currentAmmo = maxAmmo;
        currentHealth = maxHealth;
        isAlive = true;
        UpdateAmmoText();
    }

    void Update()
    {
        if (isAlive)
        {
            elapsedTime += Time.deltaTime;
            if (Keyboard.current.rKey.isPressed)
            {
                currentAmmo = 0;
                UpdateAmmoText();
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && isAlive)
        {
            Shoot();
        }
    }

    void UpdateAmmoText()
    {
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
        else
        {
            ammoText.text = currentAmmo + "/" + maxAmmo;
        }
    }

    void Shoot()
    {
        if (firePoint == null) return;
        if (currentAmmo <= 0) return;

        Ray cameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        Vector3 targetPoint;

        if (Physics.Raycast(cameraRay, out RaycastHit camHit, shootRange, shootableLayers))
        {
            targetPoint = camHit.point;
        }
        else
        {
            targetPoint = cameraRay.origin + cameraRay.direction * shootRange;
        }
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        float finalInaccuracy = baseInaccuracy;

        if (!Mouse.current.rightButton.isPressed)
            finalInaccuracy += movementInaccuracy;
        else
            finalInaccuracy -= aimInaccuracy;

        Quaternion spreadRot = Quaternion.Euler(
            Random.Range(-finalInaccuracy, finalInaccuracy),
            Random.Range(-finalInaccuracy, finalInaccuracy),
            0f
        );
        Vector3 inaccurateDir = spreadRot * direction;
        Debug.DrawRay(firePoint.position, inaccurateDir * shootRange, Color.yellow, 1.5f);


        if (Physics.Raycast(firePoint.position, inaccurateDir, out RaycastHit hit, shootRange, shootableLayers))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("hit");
                EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    int damage = Random.Range(35, 71);
                    enemy.TakeDamage(damage);
                }
            }
        }
        else
        {

        }

        currentAmmo--;
        UpdateAmmoText();
    }

    IEnumerator Reload()
    {
        ammoText.text = "RELOADING";
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        isAlive = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        currentHealth = 0;
        FindObjectOfType<GameManager>()?.TriggerGameOver();
    }
}