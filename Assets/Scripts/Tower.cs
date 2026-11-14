using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public Image healthBarFill; // la Image tipo Filled de la barra

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // no baja de 0
        UpdateHealthBar();

        Debug.Log("Torre: " + currentHealth + "/" + maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("La torre ha caído");
            // Aquí irá el game over
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}