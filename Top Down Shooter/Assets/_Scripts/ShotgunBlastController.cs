using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBlastController : MonoBehaviour
{
    [SerializeField] private BulletData pellet;
    [SerializeField] private int numPelletWaves;
    [SerializeField] private float pelletDegrees;

    private void OnEnable()
    {
        SpawnPellets();
    }

    private void SpawnPellets()
    {
        for(int i = -numPelletWaves; i <= numPelletWaves; i++)
        {
            Vector3 dir = transform.forward;
            dir += (transform.right * pelletDegrees * i);
            BulletSpawner.instance.SpawnBullet(pellet, dir, this.transform.position, 0);
        }
        //Debug.Break();
        
    }
}
