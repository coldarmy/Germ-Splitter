using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashController : MonoBehaviour
{
    public float dashSpeed;
    [SerializeField] private float dashEnergy, dashTime;
    public bool dashing;
    private float dashCounter;
    private PlayerEnergyController _energy;

    private void OnEnable()
    {
        _energy = GetComponent<PlayerEnergyController>();
    }

    private void Update()
    {
        if(dashing)
        {
            dashCounter += Time.deltaTime;
            if(dashCounter >= dashTime)
            {
                dashing = false;
            }
        }
    }

    public bool CanDash()
    {
        Debug.Log("dash ebergyt: " + dashEnergy);
        Debug.Log("cur energy: " + _energy.curEnergy);
        return dashEnergy <= _energy.curEnergy;
    }

    public void StartDash()
    {
        dashCounter = 0;
        dashing = true;
        _energy.ChangeEnergy(-dashEnergy);
    }


}
