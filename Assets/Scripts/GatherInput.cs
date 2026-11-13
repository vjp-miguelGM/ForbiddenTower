using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    
    private Controls mycontrols;
    public float valueX;

    private void Awake()
    {
        mycontrols = new Controls();//Creamos los controles
    }

    private void OnEnable()
    {
        mycontrols.Player.Enable();//Habilitamos los controles

        mycontrols.Player.Move.performed += StartMove;
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
}
