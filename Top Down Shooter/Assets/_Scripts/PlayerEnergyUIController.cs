using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyUIController : MonoBehaviour
{
    [SerializeField]private Image fill, weaponIcon1, weaponIcon2;
    [SerializeField] private float bounds;
    [SerializeField] private Color[] colors;
    private float targetAmt, weapon1Energy, weapon2Energy;
    private PlayerEnergyController PlayerEnergy;
    private void OnEnable()
    {
        PlayerEnergyController.OnEnergyChanged += HandleEnergyChange;
    }

    private void Start()
    {
        PlayerEnergy = PlayerController.instance.GetComponent<PlayerEnergyController>();
        weapon1Energy = PlayerController.instance.GetComponent<GunController>().GetSpecialWeaponEenergy(1);
        weapon2Energy = PlayerController.instance.GetComponent<GunController>().GetSpecialWeaponEenergy(2);
        weaponIcon1.transform.position += Vector3.right * ((weapon1Energy / 100f) * bounds);
        weaponIcon2.transform.position += Vector3.right * ((weapon2Energy / 100f) * bounds);
        weaponIcon1.color = colors[1];
        weaponIcon2.color = colors[1];
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
        if(weapon1Energy <= PlayerEnergy.curEnergy)
        {
            weaponIcon1.color = colors[0];
        }
        else
        {
            weaponIcon1.color = colors[1];
        }
        if (weapon2Energy <= PlayerEnergy.curEnergy)
        {
            weaponIcon2.color = colors[0];
        }
        else
        {
            weaponIcon2.color = colors[1];
        }
    }
}
