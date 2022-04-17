using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public enum pickupType
    {
        xp,
        health,
        energy
    }

    public pickupType _type;
    public int numPickups;    
    public float energyAmt;
    [SerializeField] private float lifetime;
    private float life;
    public bool fading;
    private Vector3 spawnPos;
    private void OnEnable()
    {
        life = 0f;
        fading = true;
        StartCoroutine(GoToSpawnPos());
    }

    private void Update()
    {
        if(!fading)
        {
            return;
        }

        life += Time.deltaTime;
        if(life >= lifetime)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void GetPickedUp()
    {
        fading = false;
        StopAllCoroutines();
    }

    public void SetSpawnDestination(Vector3 destination)
    {
        spawnPos = destination;
    }

    private IEnumerator GoToSpawnPos()
    {
        Vector3 startPos = transform.position;
        float t = 0;
        while(t < 1)
        {
            transform.position = Vector3.Lerp(startPos, spawnPos, t);
            t += Time.deltaTime * 5f;
            yield return null;
        }
        transform.position = spawnPos;
    }

    
}
