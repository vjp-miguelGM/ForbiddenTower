using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    public float stopDistance = 1f;
    public bool spriteFacesRight = true; // desmárcalo si el sprite original mira a la izquierda

    private Transform target;
    private Rigidbody2D rb;
    private Animator animator;
    private bool hasAnimator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hasAnimator = animator != null;
    }

    private void Start()
    {
        GameObject tower = GameObject.FindWithTag("Tower");
        if (tower != null)
        {
            target = tower.transform;
        }
        else
        {
            Debug.LogWarning("EnemyMovement: no se encontró ningún objeto con el tag Tower");
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        float distanceX = target.position.x - transform.position.x;
        float moveX = 0f;

        if (Mathf.Abs(distanceX) > stopDistance)
        {
            moveX = Mathf.Sign(distanceX);
        }

        FaceTarget(distanceX);
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
    }

    private void Update()
    {
        if (hasAnimator)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        }
    }

    // Mira siempre hacia la torre, se mueva o no
    private void FaceTarget(float distanceX)
    {
        if (Mathf.Abs(distanceX) < 0.01f) return;

        float dir = Mathf.Sign(distanceX);
        float facing = spriteFacesRight ? dir : -dir;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facing;
        transform.localScale = scale;
    }

    public bool IsNearTarget()
    {
        if (target == null) return false;
        return Mathf.Abs(target.position.x - transform.position.x) <= stopDistance;
    }
}