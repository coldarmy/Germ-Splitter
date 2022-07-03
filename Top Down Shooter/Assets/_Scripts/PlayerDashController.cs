using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashController : MonoBehaviour
{
    public float dashSpeed, maxBoostSpeed, curBoostSpeed;
    [SerializeField] private float boostAcceleration, boosterDeceleration;
    [SerializeField] private float dashEnergy, dashTime, boostEnergy;
    public bool dashing;
    private float dashCounter;
    private PlayerEnergyController _energy;

    private void OnEnable()
    {
        _energy = GetComponent<PlayerEnergyController>();
    }

    private void Update()
    {
      /*  if(dashing)
        {
            dashCounter += Time.deltaTime;
            if(dashCounter >= dashTime)
            {
                dashing = false;
            }
        }*/

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(CanBoost())
            {                
                dashing = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            dashing = false;
        }

        if(dashing)
        {
            SpendBoostEnergy();
            if(CanBoost())
            {
                AdjustBoostSpeed(true);
            }
            else
            {
                dashing = false;
            }
        }
        else
        {
            
            AdjustBoostSpeed(false);
        }
    }

    public bool CanDash()
    {
        //Debug.Log("dash ebergyt: " + dashEnergy);
       // Debug.Log("cur energy: " + _energy.curEnergy);
        return dashEnergy <= _energy.curEnergy;
    }

    public void StartDash()
    {
        dashCounter = 0;
        dashing = true;
        _energy.ChangeEnergy(-dashEnergy);
    }

    public bool CanBoost()
    {
      //  Debug.Log("boost ebergyt: " + boostEnergy * Time.deltaTime);
      //   Debug.Log("cur energy: " + _energy.curEnergy);
        return boostEnergy * Time.deltaTime <= _energy.curEnergy;
    }

    public void SpendBoostEnergy()
    {
        _energy.ChangeEnergy(-boostEnergy * Time.deltaTime);
    }

    private void AdjustBoostSpeed(bool increasingSpeed)
    {
        if(increasingSpeed)
        {
            curBoostSpeed = Mathf.Lerp(curBoostSpeed, maxBoostSpeed, boostAcceleration * Time.deltaTime);
        }
        else
        {
            curBoostSpeed = Mathf.Lerp(curBoostSpeed, 1f, boosterDeceleration * Time.deltaTime);
        }
    }

}
