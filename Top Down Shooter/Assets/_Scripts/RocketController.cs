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
    

    private void OnDisable()
    {
        Debug.Log("asking to spawn explosion");
        ObjectPoolManager.instance.SpawnExplosion(transform.position);
    }
}
