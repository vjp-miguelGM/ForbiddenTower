using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public float maxHealth;
    public float health;
    private BoxCollider2D boxcollider2D;
    private GatherInput gatherInput;
    public bool canTakeDamage= true;
    private Animator animator;
    private static readonly int damageHash = Animator.StringToHash("damage");
    private static readonly int deathHash = Animator.StringToHash("death");
    private PlayerMoveControls playerMoveControls;
    private Image imageRedLifeBar;

    public void UpdateHealthBar()
    {
        imageRedLifeBar.fillAmount = health / maxHealth;// para que se ponga el valor 
    }

    private void Awake()
    {
        imageRedLifeBar = GameObject.FindGameObjectWithTag("RedLifeBar").GetComponent<Image>();
        boxcollider2D = GetComponent<BoxCollider2D>();
        gatherInput = GetComponentInParent<GatherInput>();
        animator = GetComponentInParent<Animator>();
        playerMoveControls = GetComponentInParent<PlayerMoveControls>();
    }
    void Start()
    {
        health = maxHealth;
        UpdateHealthBar(); //AL empezar actuliazamos la barra de vida 
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage)
        {
            
            health -= damage;
            UpdateHealthBar();
            playerMoveControls.hasControl = false;// Le quito los controles 
            if (health <= 0)
            {
                Debug.Log("El jugador esta muerto");
                animator.SetBool(deathHash , true);
                boxcollider2D.enabled = false;
                gatherInput.DisableControls();
            }
            else //Si no muere
            {
                animator.SetTrigger(damageHash);
                StartCoroutine(Invulnerability());
                //StartCoroutine(HasControl());
            }
        }
    }

    private IEnumerator Invulnerability()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true; 
        playerMoveControls.hasControl = true;//devuelvo controles esta lienea si quiero la otra corrutina lo borro
    }
    //Corrutina para devolver los controles 

   // private IEnumerator HasControl()
    //{
      //  yield return new WaitForSeconds(0.5f);
        //playerMoveControls.hasControl = true;//devuelvo controles
    //}
    public void ChangeHealth(int itemValue)
    {
      health += itemValue;
      if (health > maxHealth)
      {
          health = maxHealth;
      }
      UpdateHealthBar();
    }
}
