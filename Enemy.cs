using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{




    public int maxHealth = 100;
    int currentHealth;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("HUrt");


        if(currentHealth <= 0)
        {
            Die();

        }
    }
   
    public void Die()
    {
        Debug.Log("Enemy died!");
        animator.SetBool("IsDead", true);
         this.enabled =false;
         GetComponent<Collider2D>().enabled = false;
         this.enabled = false;
         

    }
}
