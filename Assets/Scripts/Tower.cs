using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    public Image healthBarFill;
    public GameObject destroyEffectPrefab;   // prefab de partículas (Smoke)
    public float gameOverDelay = 1f;         // segundos para ver la explosión
    
    [HideInInspector] public float damageMultiplier = 1f;
    public bool IsFullHealth => currentHealth >= maxHealth;

    private bool destroyed;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (destroyed) return;

        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(damage * damageMultiplier));
        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();
        // ... el resto igual

        if (currentHealth <= 0)
        {
            StartCoroutine(DestroyTowerRoutine());
        }
    }

    private IEnumerator DestroyTowerRoutine()
    {
        destroyed = true;

        if (destroyEffectPrefab != null)
        {
            Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        }

        // Oculta la torre sin destruirla (funciona aunque el sprite esté en un hijo)
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = false;
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        // Espera para que se vea la explosión
        yield return new WaitForSeconds(gameOverDelay);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            Debug.LogError("Tower: no hay ningún GameManager en la escena");
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }
    
    public void Heal(int amount)
    {
        if (destroyed) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthBar();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount; // la barra no se queda más vacía de lo que estaba
        UpdateHealthBar();
    }
}