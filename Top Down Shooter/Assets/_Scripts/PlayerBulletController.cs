using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    private BulletController myBulletController;

    private void OnEnable()
    {
        if (myBulletController == null)
        {
            myBulletController = GetComponent<BulletController>();
        }
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hit enemy");
            collision.gameObject.gameObject.GetComponent<EnemyHP>().TakeDamage(myBulletController.myBulletData.damage);
            myBulletController.TurnOffBullet();
        }
    }
}
