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

    private bool destroyed;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (destroyed) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();

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
}