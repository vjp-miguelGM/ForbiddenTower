using System;
using UnityEngine;

public class PlayerMoveControls : MonoBehaviour
{

    public float speed; //valor configurable en el inspector, velocidad del player
    
    private GatherInput  gatherInput;
    private Rigidbody2D rigidbody2D;

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
        rigidbody2D.linearVelocity = new Vector2(speed*gatherInput.valueX,rigidbody2D.linearVelocity.y);
    }
}
