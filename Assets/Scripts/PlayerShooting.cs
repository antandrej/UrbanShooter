using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public float shootRange = 100f;
    public LayerMask shootableLayers;

    public float maxAmmo;
    public float currentAmmo;
    public Text ammoText;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void UpdateAmmoText()
    {
        if (currentAmmo <= 0)
        {
            ammoText.text = "RELOADING";
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


        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, shootRange, shootableLayers))
        {
            if (hit.collider.tag == "Enemy")
            {
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
        yield return new WaitForSeconds(3);
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }
}