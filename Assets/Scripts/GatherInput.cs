using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class GatherInput : MonoBehaviour
{
    
    private Controls mycontrols;
    public float valueX;
    public bool jumpInput;
    public bool attackInput;
    private void Awake()
    {
        mycontrols = new Controls();//Creamos los controles
        valueX = 0f;
    }

    private void OnEnable()
    {
        mycontrols.Player.Enable();//Habilitamos los controles

        mycontrols.Player.Move.performed += StartMove;
        mycontrols.Player.Move.canceled += StopMove;
        
        //Ponemos el salto
        mycontrols.Player.Jump.performed += StartJump;
        mycontrols.Player.Jump.canceled += StopJump;

        //Ponemos el ataque (tecla F)
        mycontrols.Player.Attack.performed += StartAttack;
    }

    private void StartJump(InputAction.CallbackContext ctx)
    {
        jumpInput = true;
    }
    private void StopJump(InputAction.CallbackContext ctx)
    {
        jumpInput = false;
    }

    private void StartAttack(InputAction.CallbackContext ctx)
    {
        attackInput = true; // PlayerAttack lo consume y lo pone a false
    }

    //Este método soluciona el problema de que se mueva sin parar
    private void StopMove(InputAction.CallbackContext obj)
    {
        valueX = 0f;
    }

    private void StartMove(InputAction.CallbackContext ctx)
    {
        valueX = ctx.ReadValue<float>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        mycontrols.Player.Disable();
        mycontrols.Player.Move.performed -= StartMove;
        mycontrols.Player.Move.canceled -= StopMove;
        mycontrols.Player.Jump.performed -= StartJump;
        mycontrols.Player.Jump.canceled -= StopJump;
        mycontrols.Player.Attack.performed -= StartAttack;
    }
}