using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    private BulletController myBulletController;

    private void OnEnable()
    {
        if(myBulletController == null)
        {
            myBulletController = GetComponent<BulletController>();
        }
    }
    // Start is called before the first frame update
    /* private void OnCollisionEnter(Collision collision)
     {
         if (collision.gameObject.gameObject.CompareTag("Player"))
         {
             collision.gameObject.gameObject.GetComponent<PlayerController>().TakeDamage(myBulletController.myBulletData.damage);
             myBulletController.TurnOffBullet();
         }
     }*/

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(myBulletController.myBulletData.damage);
            myBulletController.TurnOffBullet();
        }
    }

}
