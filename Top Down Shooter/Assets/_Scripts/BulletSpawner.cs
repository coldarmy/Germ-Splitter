using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner instance;
    [SerializeField] private BulletData[] bullets;
    [SerializeField] private float offset;
    private int bulletPoolSize, bulletCount;
    private BulletController[] bulletPool;
    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
        InitBullets();
    }

    public void InitBullets() // guncontroller calls this on start
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bulletPoolSize += bullets[i].poolSize;
        }
        bulletCount = 0;
        bulletPool = new BulletController[bulletPoolSize];

        for (int i = 0; i < bullets.Length; i++)
        {
            for(int j = 0; j < bullets[i].poolSize; j++)
            {
                GameObject g = Instantiate(bullets[i].bulletObject, this.transform);
                g.SetActive(false);
                bulletPool[bulletCount] = g.GetComponent<BulletController>();
                bulletCount++;
            }
        }
    }

    public void SpawnBullet(BulletData data, Vector3 lookDir, Vector3 pos, float offset)
    {
        for (int i = 0; i < bulletPool.Length; i++)
        {
            if(!bulletPool[i].isActiveAndEnabled)
            {
                if(bulletPool[i].myBulletData == data)
                {
                    bulletPool[i].transform.forward = lookDir;
                    bulletPool[i].transform.position = pos;
                    bulletPool[i].transform.position += (bulletPool[i].transform.forward * offset);
                    bulletPool[i].gameObject.SetActive(true);
                    break;
                }
                else
                {
                  //  Debug.Log(data.bulletObject.name + "doesnt match " + bulletPool[i].myBulletData.bulletObject.name);
                }                
            }
            else
            {
                //Debug.Log("no inactive bullets of this type");
            }
        }
    } 
}
