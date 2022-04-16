using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    [SerializeField] GameObject explosion;
    [SerializeField] private int poolSize;
    private ObjectPool<GameObject> _explosionPool;
    private Vector3 spawnPos;

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
        _explosionPool = new ObjectPool<GameObject>(CreateExplosion, actionOnGet:OnTakeExplosionFromPool, actionOnRelease:OnReturnExplosionToPool, collectionCheck: true, defaultCapacity: poolSize);

    }

    private GameObject CreateExplosion()
    {
        var ex = Instantiate(explosion);
        ex.GetComponent<Poolable>().SetPool(_explosionPool);
        return ex;
    }

    private void OnReturnExplosionToPool(GameObject g)
    {
        g.SetActive(false);  
    
    }

    private void OnTakeExplosionFromPool(GameObject g)
    {
        g.transform.position = spawnPos;
        g.SetActive(true);

    }

    public void SpawnExplosion(Vector3 pos)
    {
        Debug.Log("spawning explosion");
        spawnPos = pos;
        _explosionPool.Get();
    }





}
