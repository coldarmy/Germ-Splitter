using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class LandmineController : PlayerBulletController
{
    [SerializeField] private MeshRenderer light;
    [SerializeField] private Material[] lightMats;
    [SerializeField] private float primingTime;
    [SerializeField] private ExplosionController regularExplosion, primedExplosion;
    private float primeCount;
    public bool primed;


    private void OnEnable()
    {
        primed = false;
        light.material = lightMats[0];
        primeCount = 0;
        base.OnEnable();
    }

    private void Update()
    {
        if(!primed)
        {
            primeCount += Time.deltaTime;
            if(primeCount >= primingTime)
            {
                GetPrimed();
            }
        }
        base.Update();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
         {
            // collision.gameObject.gameObject.GetComponent<EnemyHP>().TakeDamage(myBulletController.myBulletData.damage);
            collision.gameObject.GetComponent<EnemyHP>().TakeDamage(myBulletData.damage, myBulletData.stunTime);

           // Debug.Log("touching enemy");

            TurnOffBullet();
        }
        
    }

    public override void SpawnBulletExplosion()
    {
        if(!primed)
        {
            GameObject exp = LeanPool.Spawn(regularExplosion.gameObject);
            exp.transform.position = this.transform.position;
        }
        else
        {
            GameObject exp = LeanPool.Spawn(primedExplosion.gameObject);
            exp.transform.position = this.transform.position;
        }
    }

    private void GetPrimed()
    {
        primed = true;
        light.material = lightMats[1];
    }
}
