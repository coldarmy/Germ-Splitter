using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private float lifeTime, growSpeed;
    public int damage;
    private float curLife;

    private void Update()
    {
        
        transform.localScale += (Vector3.one * growSpeed * Time.deltaTime);
        curLife += Time.deltaTime;
        if(curLife >= lifeTime)
        {
            Destroy(this.gameObject);
        }

    }

    public void SpawnExplosion(int dmg)
    {
        damage = dmg;
        curLife = 0;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
    }



}
