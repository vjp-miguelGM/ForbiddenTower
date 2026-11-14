using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    public float knockbackDuration = 0.25f; // tiempo que el enemigo no controla su movimiento

    private int currentHealth;
    private Rigidbody2D rb;
    private float knockbackTimer;

    public bool IsKnockedBack => knockbackTimer > 0f;

    private void Awake()
    {
        
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (knockbackTimer > 0f) knockbackTimer -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Knockback(Vector2 direction, float force)
    {
        if (rb == null) return;

        knockbackTimer = knockbackDuration;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    private void Die()
    {
        if (WaveManager.Instance != null) WaveManager.Instance.EnemyDied();
        Destroy(gameObject);
    }
}