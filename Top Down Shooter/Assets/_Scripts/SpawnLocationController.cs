using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocationController : MonoBehaviour
{
    public delegate void SpawnLocationEvent(Vector3 pos, Vector3 offsetRadius);
    public static SpawnLocationEvent OnSpawnLocationReady;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        OnSpawnLocationReady?.Invoke(transform.position, offset);
    }

}
