using UnityEngine;
using UnityEngine.InputSystem;
public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float launchForce = 18f;
    public float fireCooldown = 0.5f;
    private float nextShotTime;
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame
        && Time.time >= nextShotTime && GameManager.Instance != null
        && GameManager.Instance.CanShoot())
        {
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject obj = Instantiate(projectilePrefab,
        firePoint.position, firePoint.rotation);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        ProjectileImpact impact = obj.GetComponent<ProjectileImpact>();
        if (rb == null || impact == null)
        {
            Debug.LogError("Projectile prefab needs Rigidbody and ProjectileImpact");
            Destroy(obj);
            return;
        }
        impact.RegisterShot();
        GameManager.Instance.ShotFired();
        rb.AddForce(firePoint.forward * launchForce, ForceMode.Impulse);
        Destroy(obj, 5f);
        nextShotTime = Time.time + fireCooldown;
    }
}