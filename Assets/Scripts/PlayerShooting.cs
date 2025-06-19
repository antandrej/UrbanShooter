using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;

    public float shootRange = 100f;

    public LayerMask shootableLayers;

    //public LineRenderer bulletTracer;
    //public GameObject hitEffectPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (firePoint == null)
        {
            Debug.LogError("Fire Point not found.");
            return;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); 
        
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, (ray.origin + ray.direction * shootRange) - firePoint.position, out hit, shootRange, shootableLayers))
        {
            Debug.Log("Igrač je pogodio: " + hit.collider.name + " na poziciji: " + hit.point + " (Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer) + ")");

            /*// Visual trace
            if (bulletTracer != null)
            {
                bulletTracer.SetPosition(0, firePoint.position);
                bulletTracer.SetPosition(1, hit.point);
                //StartCoroutine(ShowTracer(bulletTracer, 0.1f));
            }

            // Hit effect
            if (hitEffectPrefab != null)
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitEffect, 2f);
            }*/
/*
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(20);
            }
            */
        }
        else
        {
            Debug.Log("Igrač je promašio! Ray je otišao do: " + (ray.origin + ray.direction * shootRange) + " (Nije pogodio ni jedan definisan sloj).");

            /*// Visual trace to the end
            if (bulletTracer != null)
            {
                bulletTracer.SetPosition(0, firePoint.position);
                bulletTracer.SetPosition(1, ray.origin + ray.direction * shootRange);
                //StartCoroutine(ShowTracer(bulletTracer, 0.1f));
            }
            */
        }
    }

    /*
    IEnumerator ShowTracer(LineRenderer tracer, float duration)
    {
        tracer.enabled = true;
        yield return new WaitForSeconds(duration);
        tracer.enabled = false;
    }
    */
}