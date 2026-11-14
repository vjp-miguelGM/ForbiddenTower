using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Tu compañero llamará a este método desde el ataque del player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Aquí podrás poner la animación de muerte antes de destruir
        Destroy(gameObject);
    }
}