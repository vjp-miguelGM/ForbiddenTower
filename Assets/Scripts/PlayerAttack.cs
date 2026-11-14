using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    public Transform attackPoint;      // Objeto vacío delante de la espada
    public float attackRange = 0.6f;   // Radio del golpe
    public int attackDamage = 10;      // Daño por golpe
    public LayerMask enemyLayers;      // Layer de los enemigos (Enemy)

    [Header("Cadencia")]
    public float attackRate = 2f;      // Ataques por segundo
    private float nextAttackTime = 0f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.time < nextAttackTime) return;

        // Clic izquierdo
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            nextAttackTime = Time.time + 1f / attackRate;

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            DealDamage();
        }
    }

    // Aplica el daño a los enemigos dentro del círculo
    public void DealDamage()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("PlayerAttack: falta asignar attackPoint en el Inspector.");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        Debug.Log("Enemigos detectados: " + hitEnemies.Length);

        foreach (Collider2D enemy in hitEnemies)
        {
            // GetComponentInParent funciona aunque el collider esté en un hijo
            EnemyHealth enemyHealth = enemy.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("Daño aplicado a: " + enemy.name);
            }
        }
    }

    // Círculo rojo en la Scene para ver el alcance
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}