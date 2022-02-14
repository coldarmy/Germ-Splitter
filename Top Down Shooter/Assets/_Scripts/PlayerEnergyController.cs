using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyController : MonoBehaviour
{
    public delegate void EnergyEvent(float energy);
    public static EnergyEvent OnEnergyChanged;
    public float curEnergy, maxEnergy;

    public void ChangeEnergy(float amt)
    {
        curEnergy += amt;
        curEnergy = Mathf.Clamp(curEnergy, 0f, maxEnergy);
        OnEnergyChanged?.Invoke(curEnergy);
    }

    public bool CanShoot(float energy)
    {
        return energy <= curEnergy;
    }
}
