using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class PlayerBulletController : BulletController
{
    public delegate void BulletEvent(ExplosionController ex);
    public static BulletEvent SpawnExplosion;
    //private BulletController myBulletController;
    [SerializeField] private ExplosionController explosion;
    private GlaiveSpawner glaiveSpawner;


    protected virtual void OnEnable()
    {
        glaiveSpawner = GetComponent<GlaiveSpawner>();
        base.OnEnable();
    }

    protected virtual void Update()
    {
        base.Update();
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.gameObject.CompareTag("Enemy"))
        {
             Debug.Log("hit enemy", this);
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
            SpawnBulletExplosion();
        }
       
        base.TurnOffBullet();
    }

    public virtual void SpawnBulletExplosion()
    {
        GameObject exp = LeanPool.Spawn(explosion.gameObject);
        exp.transform.position = this.transform.position;
        //ObjectPoolManager.instance.SpawnExplosion(transform.position);
    }
}
