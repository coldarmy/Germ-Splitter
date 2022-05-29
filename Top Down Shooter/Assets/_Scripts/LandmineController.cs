using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmineController : PlayerBulletController
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
         {
            // collision.gameObject.gameObject.GetComponent<EnemyHP>().TakeDamage(myBulletController.myBulletData.damage);
            collision.gameObject.GetComponent<EnemyHP>().TakeDamage(myBulletData.damage, myBulletData.stunTime);

            Debug.Log("touching enemy");

            TurnOffBullet();
        }
        
    }
}
