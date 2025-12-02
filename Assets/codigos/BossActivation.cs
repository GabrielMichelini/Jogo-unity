using UnityEngine;

public class BossActivation : MonoBehaviour
{
    public BossHealth bossScript; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Player"))
        {
          
            if (bossScript != null)
            {
                bossScript.ShowHealthBar();
            }

           
            gameObject.SetActive(false);
        }
    }
}