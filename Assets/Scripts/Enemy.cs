using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  
    public float health;
    protected Rigidbody2D rigidbody2D;
    protected Animator animator;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
