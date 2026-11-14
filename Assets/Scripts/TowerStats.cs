using UnityEngine;

public class Tower : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Torre: " + currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("La torre ha caído");
            // Aquí irá el game over
        }
    }
}