using System;
using UnityEngine;

public class PlayerMoveControls : MonoBehaviour
{

    public float speed; //valor configurable en el inspector, velocidad del player
    public float jumpForce;
    
    private GatherInput  gatherInput;
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    private int direction = 1;
    public float rayLength; //Longitud de los rayos que lanzaremos hacia abajo
    public LayerMask groundLayer; //Para seleccionar en el inspector la capa Ground
    public Transform leftPoint; //Para pasarle en el inspector el gameobject Left Point de la jerarquía
    public Transform rightPoint; //Para pasarle en el inspector el gameobject Right Point de la jerarquía
    private bool grounded; //Para guardar si estamos tocando suelo o no
      private void Flip()
    {
        if (gatherInput.valueX * direction <0)//mover el sprite hacia la dirección que lo mueve el player
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);    
            direction = (int)Mathf.Sign(gatherInput.valueX);
        }
    }
    private void CheckStatus()
    {//Lanzamos dos rayos hacia abajo
        RaycastHit2D leftCheckHit = Physics2D.Raycast(leftPoint.position, Vector2.down, rayLength, groundLayer);
        RaycastHit2D rightCheckHit = Physics2D.Raycast(rightPoint.position, Vector2.down, rayLength, groundLayer);
        if (leftCheckHit || rightCheckHit) //Si uno de los dos rayos ha chocado con la capa ground
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
    private void Awake()
    {
        gatherInput = GetComponent<GatherInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Jump()
    {
        if (grounded == true)
        {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x,jumpForce);
        }

        grounded = false;
    }

    private void SetAnimatorValues()
    {
        animator.SetFloat("Speed",Math.Abs(rigidbody2D.linearVelocity.x)) ;
        animator.SetFloat("vSpeed", rigidbody2D.linearVelocity.y);
        animator.SetBool("isGrounded", grounded);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimatorValues();
    }

    private void FixedUpdate()
    {
        CheckStatus();
        Move();
        Jump();
    }

    private void Move()
    {
        Flip();
        rigidbody2D.linearVelocity = new Vector2(speed*gatherInput.valueX,rigidbody2D.linearVelocity.y);
        animator.SetFloat("Speed", rigidbody2D.linearVelocity.x);
    }
}
