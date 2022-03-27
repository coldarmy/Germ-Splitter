using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private float lifeTime, growSpeed;
    public int damage;
    private float curLife;
    private Poolable _poolable;

    private void OnEnable()
    {
        if(_poolable == null)
        {
            _poolable = GetComponent<Poolable>();
        }

        SpawnExplosion(damage);
    }

    private void Update()
    {
        
        transform.localScale += (Vector3.one * growSpeed * Time.deltaTime);
        curLife += Time.deltaTime;
        if(curLife >= lifeTime)
        {
            Disable();
        }

    }

    public void SpawnExplosion(int dmg)
    {
        damage = dmg;
        curLife = 0;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
    }

    private void Disable()
    {
        //this.gameObject.SetActive(false);
        _poolable.ReleaseToPool();
    }


}
