using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private float lifeTime, explosioinForce;
    [SerializeField] private Vector3 finalScale;
    public int damage;
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
        
      /*  transform.localScale += (Vector3.one * growSpeed * Time.deltaTime);
        curLife += Time.deltaTime;
        if(curLife >= lifeTime)
        {
            Disable();
        }*/

    }

    public void SpawnExplosion(int dmg)
    {
        damage = dmg;
      //  curLife = 0;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        float radius = finalScale.x;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(explosioinForce, transform.position, radius);
        }
        transform.DOScale(finalScale, lifeTime).OnComplete(() =>
        {
           gameObject.SetActive(false);
        });

    }

    private void Disable()
    {
        //this.gameObject.SetActive(false);
        _poolable.ReleaseToPool();
    }


}
