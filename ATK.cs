using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ATK : MonoBehaviour
{



    public Animator animeatk;
    public float ATKrange = 0.5f;
    public LayerMask emnemyLayer;
    public Transform ATKpoint;
    public int ATKdamage = 40;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.J))
     {
        ATkk();

     }

    }

    void ATkk()
    {
        animeatk.SetTrigger("ATK");

       Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ATKpoint.position, ATKrange, emnemyLayer);


    foreach(Collider2D enemy in hitEnemies)
    {
        Debug.Log("We hit" + enemy.name);
        enemy.GetComponent<Enemy>().TakeDamage(ATKdamage);

    }

    }
    void OnDrawGizmos()
    {
        if(ATKpoint == null)
        return;

        Gizmos.DrawWireSphere(ATKpoint.position, ATKrange);

    }


}
