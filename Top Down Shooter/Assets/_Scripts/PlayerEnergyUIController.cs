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
    [SerializeField]private float targetAmt, weapon1Energy, weapon2Energy;
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
       
        Vector3 newIconPos1 = weaponIcon1.transform.localPosition;
        newIconPos1.x += weapon1Energy;
        weaponIcon1.transform.localPosition = newIconPos1;

        Vector3 newIconPos2 = weaponIcon2.transform.localPosition;
        newIconPos2.x += weapon2Energy;
        weaponIcon2.transform.localPosition = newIconPos2;

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
