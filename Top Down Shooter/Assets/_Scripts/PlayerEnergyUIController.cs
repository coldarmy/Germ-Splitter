using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyUIController : MonoBehaviour
{
    [SerializeField]private Image fill;
    private float targetAmt;
    private PlayerEnergyController PlayerEnergy;
    private void OnEnable()
    {
        PlayerEnergyController.OnEnergyChanged += HandleEnergyChange;
    }

    private void Start()
    {
        PlayerEnergy = PlayerController.instance.GetComponent<PlayerEnergyController>();
    }

    private void Update()
    {
        if(PlayerEnergy != null)
        {
            fill.fillAmount = Mathf.Lerp(fill.fillAmount, targetAmt, Time.deltaTime * 8f);
        }
        
    }

    private void OnDisable()
    {
        PlayerEnergyController.OnEnergyChanged -= HandleEnergyChange;
    }

    private void HandleEnergyChange(float energy)
    {
        targetAmt = (PlayerEnergy.curEnergy / PlayerEnergy.maxEnergy);
    }
}
