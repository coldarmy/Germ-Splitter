using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Poolable : MonoBehaviour
{
    private ObjectPool<GameObject> _pool;

    public void SetPool(ObjectPool<GameObject> pool) => _pool = pool;

    public void ReleaseToPool()
    {
        _pool.Release(this.gameObject);
    }

}
