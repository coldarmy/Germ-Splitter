using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : BulletController
{
    public delegate void BulletEvent(ExplosionController ex);
    public static BulletEvent SpawnExplosion;
    //private BulletController myBulletController;
    [SerializeField] ExplosionController explosion;
    private GlaiveSpawner glaiveSpawner;
    

    private void OnEnable()
    {
        glaiveSpawner = GetComponent<GlaiveSpawner>();
        base.OnEnable();
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log("hit enemy");
            // collision.gameObject.gameObject.GetComponent<EnemyHP>().TakeDamage(myBulletController.myBulletData.damage);
            collision.gameObject.GetComponent<EnemyHP>().TakeDamage(myBulletData.damage, myBulletData.stunTime);
            
            Debug.Log("touching enemy");
            if (glaiveSpawner != null)
            {
                glaiveSpawner.SpawnGlaive(collision.transform);
            }
            TurnOffBullet();
        }
    }

    public override void TurnOffBullet()
    {
        if(explosion != null)
        {
            // SpawnExplosion?.Invoke(explosion);
            ObjectPoolManager.instance.SpawnExplosion(transform.position);
        }
       
        base.TurnOffBullet();
    }
}
