using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    
    public GameObject attackHitbox;
    
    
    public float attackDuration = 0.2f;

    void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
         
            StartCoroutine(DoAttack());
        }
    }

   
    private IEnumerator DoAttack()
    {
     
        attackHitbox.SetActive(true);

    
        yield return new WaitForSeconds(attackDuration);

       
        attackHitbox.SetActive(false);
    }
}