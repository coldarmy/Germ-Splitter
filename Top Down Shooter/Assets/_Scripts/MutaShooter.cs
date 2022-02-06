using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutaShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private GameObject[] bullets;
    

    private void OnEnable()
    {
        bullets = new GameObject[10];
        for(int i = 0; i < bullets.Length; i++)
        {
            GameObject b = Instantiate(bulletPrefab.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
