using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    private Controls myControls; 
    public float valuex;
    //public bool jumpImput;
    public bool jumpPressed;
    public bool jumpHeld;
    private void Awake()
    {
        myControls = new Controls();//Creamos los controles 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        myControls.Player.Enable(); //Habilitamos los controloes 
        myControls.Player.Move.performed += StartMove;
        myControls.Player.Move.canceled += StopMove;
        myControls.Player.Jump.performed += StartJump;
        myControls.Player.Jump.canceled += StopMoveJump;
    }

    private void StartJump(InputAction.CallbackContext obj)
    {
        //jumpImput = true;
        jumpPressed = true;
        jumpHeld = true;
    }
    private void StopMoveJump(InputAction.CallbackContext obj)
    {
        //jumpImput = false;
        jumpHeld = false;
    }
    

    private void OnDisable()
    {
        myControls.Player.Disable();//Los deshabilitamos para cuanndo el personaje muera 
        myControls.Player.Move.performed -= StartMove;
        myControls.Player.Move.canceled -= StopMove;
        myControls.Player.Jump.performed -= StartJump;
        myControls.Player.Jump.canceled -= StopMoveJump;
    }

    private void StartMove(InputAction.CallbackContext ctx)
    {
        valuex = ctx.ReadValue<float>();
        if (valuex > 0.1f) valuex = 1;
        else if (valuex < -0.1f) valuex = -1;
        else valuex = 0;
        
    } 
    private void StopMove(InputAction.CallbackContext ctx)
         {
             valuex = 0f;//AL soltar reduce la velocidad a cero  
         }

    public void DisableControls()
    {
        myControls.Player.Disable();//Los deshabilitamos para cuanndo el personaje muera 
        myControls.Player.Move.performed -= StartMove;
        myControls.Player.Move.canceled -= StopMove;
        myControls.Player.Jump.performed -= StartJump;
        myControls.Player.Jump.canceled -= StopMoveJump;
        valuex = 0;
    }
}
