using System;
using UnityEngine;

public class PlayerMoveControls : MonoBehaviour
{

    public float speed; //valor configurable en el inspector, velocidad del player
    
    private GatherInput  gatherInput;
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    private int direction = 1;


    private void Flip()
    {
        if (gatherInput.valueX * direction <0)//mover el sprite hacia la dirección que lo mueve el player
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);    
            direction = (int)Mathf.Sign(gatherInput.valueX);
        }
    }

    private void Awake()
    {
        gatherInput = GetComponent<GatherInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void SetAnimatorValues()
    {
        animator.SetFloat("Speed",Math.Abs(rigidbody2D.linearVelocity.x)) ;
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
        Move();
    }

    private void Move()
    {
        Flip();
        rigidbody2D.linearVelocity = new Vector2(speed*gatherInput.valueX,rigidbody2D.linearVelocity.y);
        animator.SetFloat("Speed", rigidbody2D.linearVelocity.x);
    }
}
