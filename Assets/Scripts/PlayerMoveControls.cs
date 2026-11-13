using System;
using UnityEngine;

public class PlayerMoveControls : MonoBehaviour
{

    public float speed; //valor configurable en el inspector, velocidad del player
    
    private GatherInput  gatherInput;
    private Rigidbody2D rigidbody2D;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Flip();
        rigidbody2D.linearVelocity = new Vector2(speed*gatherInput.valueX,rigidbody2D.linearVelocity.y);
    }
}
