using UnityEngine;
public class ProjectileImpact : MonoBehaviour
{
    private bool registered;
    private bool reported;
    public void RegisterShot() { registered = true; }
    void OnCollisionEnter(Collision collision)
    {
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            target.Hit();
            Destroy(gameObject);
        }
    }
    void OnDestroy()
    {
        if (!registered || reported) return;
        reported = true;
        if (GameManager.Instance != null)
            GameManager.Instance.ProjectileFinished();
    }
}