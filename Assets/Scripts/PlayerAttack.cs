using UnityEngine;

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
    private GatherInput gatherInput;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gatherInput = GetComponent<GatherInput>();
    }

    private void Update()
    {
        // Si pulsas F durante el cooldown no se acumula el ataque
        if (Time.time < nextAttackTime)
        {
            gatherInput.attackInput = false;
            return;
        }

        // Tecla F (definida en el asset Controls, acción Attack)
        if (gatherInput.attackInput)
        {
            gatherInput.attackInput = false; // consumimos el input
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