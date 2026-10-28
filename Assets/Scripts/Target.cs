using UnityEngine;

public class Target : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    private bool isAlive = false;

    void Awake()
    {
        // Save initial transform states so we can reset them on respawn
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    void Start()
    {
        // Initial setup for the start of the game
        Respawn();
    }

    public void Respawn(Vector3? customPosition = null)
    {
        // Reset transform
        if (customPosition.HasValue)
        {
            transform.position = customPosition.Value;
        }
        else
        {
            transform.position = initialPosition;
        }

        transform.rotation = initialRotation;
        transform.localScale = initialScale;

        // Reset stats and make active
        currentHealth = maxHealth;
        isAlive = true;
        gameObject.SetActive(true);
    }

    public void Hit()
    {
        if (!isAlive || currentHealth <= 0) return;

        currentHealth--;

        if (currentHealth > 0)
        {
            // Shrink target slightly when hit
            transform.localScale *= 0.9f;
        }
        else
        {
            // Target is dead/destroyed for this wave
            isAlive = false;
            gameObject.SetActive(false); // Hide instead of destroying

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }
        }
    }
}