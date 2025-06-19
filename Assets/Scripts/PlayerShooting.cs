using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;

    public float shootRange = 100f;

    public LayerMask shootableLayers;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }


    void Shoot()
    {
        if (firePoint == null) return;

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

        Debug.DrawRay(firePoint.position, direction * shootRange, Color.red, 2f);

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, shootRange, shootableLayers))
        {
            Debug.Log("Hit: " + hit.collider.name + " at " + hit.point);
            if (hit.collider.tag == "Enemy")
            {
                EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    int damage = Random.Range(35, 71);
                    enemy.TakeDamage(damage);
                    Debug.Log("Enemy took " + damage + " damage!");
                }
            }
        }
        else
        {
            Debug.Log("Missed. Ray ended at: " + (firePoint.position + direction * shootRange));
        }
    }

}