using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlaiveSpawner : MonoBehaviour
{
    [SerializeField] private GameObject glaiveObj;

    public void SpawnGlaive(Transform hitEnemy)
    {
        GameObject glaive = LeanPool.Spawn(glaiveObj);
        GlaiveController g = glaive.GetComponent<GlaiveController>();
        g.transform.position = hitEnemy.transform.position;
        g.gameObject.SetActive(true);
        g.Spawn(hitEnemy);
    }
}
