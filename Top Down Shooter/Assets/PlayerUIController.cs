using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image redFlash;
    private float redFlashTime = .15f;

    private void OnEnable()
    {
        PlayerController.OnPlayerTakesDamage += HandlePlayerDamage;        
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerTakesDamage -= HandlePlayerDamage;
    }

    private void HandlePlayerDamage(int damage)
    {
        hpText.text = "HP: " + damage;
        StartCoroutine(FlashScreenRed());
    }

    private IEnumerator FlashScreenRed()
    {
        WaitForSeconds w = new WaitForSeconds(redFlashTime);
        redFlash.enabled = true;
        yield return w;
        redFlash.enabled = false;
    }
}
