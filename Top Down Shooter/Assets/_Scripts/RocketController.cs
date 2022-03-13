using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{

    [SerializeField] ExplosionController explosionPrefab;
    private BulletController myBulletController;

    

    private void OnEnable()
    {
        if (myBulletController == null)
        {
            myBulletController = GetComponent<BulletController>();
        }
    }

    public void SpawnExplosion()
    {
        ExplosionController ex = Instantiate(explosionPrefab);
        ex.transform.position = transform.position;
        ex.transform.rotation = transform.rotation;
        ex.SpawnExplosion(myBulletController.myBulletData.damage);
    }

    private void OnDisable()
    {
        
    }
}
