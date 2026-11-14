using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 1.5f;

    private EnemyMovement movement;
    private Tower tower;
    private Animator animator;
    private bool hasAnimator;
    private float timer;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();
        hasAnimator = animator != null;
    }

    private void Start()
    {
        GameObject towerObj = GameObject.FindWithTag("Tower");
        if (towerObj != null)
        {
            tower = towerObj.GetComponent<Tower>();
        }
        else
        {
            Debug.LogWarning("EnemyAttack: no se encontró ningún objeto con el tag Tower");
        }
        timer = 0f;
    }

    private void Update()
    {
        if (tower == null) return;

        timer -= Time.deltaTime;

        if (movement.IsNearTarget() && timer <= 0f)
        {
            Attack();
            timer = attackCooldown;
        }
    }

    private void Attack()
    {
        if (hasAnimator)
        {
            animator.SetTrigger("Attack");
        }
        tower.TakeDamage(damage);
    }
}