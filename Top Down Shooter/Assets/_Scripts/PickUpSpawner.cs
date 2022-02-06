using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private PickUpController[] pickupPrefabs;
    private List<PickUpController> _pickups;
    [SerializeField] private int energyLow, energyHigh;
    [SerializeField] private float hpChance;
    private float xOffset, zOffset;
    

    private void OnEnable()
    {
        EnemyHP.OnEnemyDeath += HandleEnemyDeath;

        _pickups = new List<PickUpController>();
        for (int i = 0; i < pickupPrefabs.Length; i++)
        {
            for (int j = 0; j < pickupPrefabs[i].numPickups; j++)
            {
                _pickups.Add(Instantiate(pickupPrefabs[i], this.transform));
            }
        }
    }

    private void OnDisable()
    {
        EnemyHP.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Vector3 pos)
    {
        
        int numEnergy = UnityEngine.Random.Range(energyLow, energyHigh + 1);
        for(int i = 0; i < numEnergy; i++)
        {
            xOffset = UnityEngine.Random.Range(-2, 2f);
            zOffset = UnityEngine.Random.Range(-2, 2f);
            pos += Vector3.right * xOffset;
            pos += Vector3.forward * zOffset;
            SpawnPickUp(PickUpController.pickupType.energy, pos);
        }
        float h = UnityEngine.Random.Range(0, 1f);
        if(h >= hpChance)
        {
            xOffset = UnityEngine.Random.Range(-2, 2f);
            zOffset = UnityEngine.Random.Range(-2, 2f);
            pos += Vector3.right * xOffset;
            pos += Vector3.forward * zOffset;
            SpawnPickUp(PickUpController.pickupType.health, pos);
        }
    }

    private void SpawnPickUp(PickUpController.pickupType t, Vector3 pos)
    {
        for(int i = 0; i < _pickups.Count; i++)
        {
            if(_pickups[i]._type == t && !_pickups[i].gameObject.activeInHierarchy)
            {
                _pickups[i].transform.position = pos;
                _pickups[i].gameObject.SetActive(true);
                
                return;
            }
        }
    }
}
