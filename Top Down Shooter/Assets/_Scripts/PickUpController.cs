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
    public bool fading;
    [SerializeField] private float lifetime;

    private float life;

    private void OnEnable()
    {
        life = 0f;
        fading = true;
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
}
