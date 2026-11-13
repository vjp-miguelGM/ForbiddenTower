using System;
using System.Collections;
using UnityEngine;

public class PlayerMoveControls : MonoBehaviour
{
    public float speed; // Cuantas unidades al segundo se mueve 
    public float jumpForce;
    private GatherInput gatherInput;
    private Rigidbody2D rigidbody2D;
    private int direction = 1;
    private SpriteRenderer SpriteRenderer;
    private Animator animator;
    private int remainigJumps;

    public bool hasControl = true;
    public float jumpCutMultipler = 0.5f;
    public int extrajumps = 1;
    public float raylength;
    public LayerMask groundLayer;
    public Transform leftPoint;
    public Transform rightPoint;
    private bool grounded;

    private static int SpeedHash = Animator.StringToHash("Speed");
    private static int VSpeedHash = Animator.StringToHash("VSpeed");
    private static int isGroundedHash = Animator.StringToHash("IsGrounded");


    private void Start()
    {
        remainigJumps = extrajumps;
    }

    private void CheckStatus()
    {
        RaycastHit2D leftCheckHit = Physics2D.Raycast(leftPoint.position, Vector2.down, raylength, groundLayer);
        RaycastHit2D rightCheckHit = Physics2D.Raycast(rightPoint.position, Vector2.down, raylength, groundLayer);
        if (leftCheckHit || rightCheckHit)
        {
            grounded = true;
            remainigJumps = extrajumps;
        }
        else
        {
            grounded = false;
        }

        SeeRays(leftCheckHit, rightCheckHit);
    }

    private void SeeRays(RaycastHit2D leftCheckHit, RaycastHit2D rightCheckHit)
    {
        Color colorLeft = leftCheckHit ? Color.green : Color.red;
        Color colorRight = rightCheckHit ? Color.green : Color.red;
        Debug.DrawRay(leftPoint.position, Vector2.down * raylength, colorLeft);
        Debug.DrawRay(rightPoint.position, Vector2.down * raylength, colorRight);
    }

    private void Awake()
    {
        gatherInput = GetComponent<GatherInput>(); //Referencia al imoputx que recoge el  input
        rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SetAnimatorValues();
    }

    private void SetAnimatorValues()
    {
        animator.SetFloat(SpeedHash,
            Math.Abs(rigidbody2D.linearVelocity.x)); //Se puede poner con magnitude pero es para lo dos ejes ;
        animator.SetFloat(VSpeedHash, rigidbody2D.linearVelocityY);
        animator.SetBool(isGroundedHash, grounded);
    }

    private void Flip()
    {
        if (direction * gatherInput.valuex < 0)
        {
            // transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (SpriteRenderer.flipX == true)
            {
                SpriteRenderer.flipX = false;
            }
            else
            {
                SpriteRenderer.flipX = true;
            }

            direction *= -1;
        }
    }

    private void Jump()
    {
        //if (gatherInput.jumpImput == true)
        if (gatherInput.jumpPressed == true)
        {
            if (grounded == true)
            {
                rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, jumpForce);
                remainigJumps = extrajumps;

            }
            else
            {
                if (remainigJumps > 0)
                {
                    remainigJumps--;
                    rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, jumpForce);
                }
            }


        }

        if (!gatherInput.jumpHeld && rigidbody2D.linearVelocity.y > 0)
        {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x,
                rigidbody2D.linearVelocity.y * jumpCutMultipler);
        }

        //gatherInput.jumpImput = false;
        gatherInput.jumpPressed = false;
    }

    // Update is called once per frame
    private void Move()
    {
        Flip();
        rigidbody2D.linearVelocity = new Vector2(speed * gatherInput.valuex, rigidbody2D.linearVelocity.y);

    }

    private void FixedUpdate() //ES para la velocidad lo que hace es mantenerla
    {
        CheckStatus();
        if (hasControl == true)
        {
            Move();
            Jump();
        }


    }

    public IEnumerator KnockBack(float forceX, float forceY, float duration, Transform transformEnemy)
    {
        int KnocBackDirection;
        if (this.transform.position.x < transformEnemy.position.x)
        {
            KnocBackDirection = -1;

        }
        else
        {
            KnocBackDirection = 1;
        }
        rigidbody2D.linearVelocity = Vector2.zero;
        Vector2 forceKnockBack = new Vector2(KnocBackDirection * forceX, forceY);
        rigidbody2D.AddForce(forceKnockBack, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        rigidbody2D.linearVelocity = Vector2.zero;
        

    }


}
