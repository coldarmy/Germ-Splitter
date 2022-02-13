using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image redFlash, greenFlash;
    private float flashTime = .15f;

    private void OnEnable()
    {
        PlayerController.OnPlayerTakesDamage += HandlePlayerDamage;
        PlayerController.OnPlayerGetsHealth += HandleHealthPickup;
    }

    

    private void OnDisable()
    {
        PlayerController.OnPlayerTakesDamage -= HandlePlayerDamage;
        PlayerController.OnPlayerGetsHealth -= HandleHealthPickup;
    }

    private void HandleHealthPickup(int hp)
    {
        hpText.text = "HP: " + hp;
        StartCoroutine(FlashScreen(greenFlash));
    }

    private void HandlePlayerDamage(int hp)
    {
        hpText.text = "HP: " + hp;
        StartCoroutine(FlashScreen(redFlash));
    }

    private IEnumerator FlashScreen(Image img)
    {
        WaitForSeconds w = new WaitForSeconds(flashTime);
        img.enabled = true;
        yield return w;
        img.enabled = false;
    }
}
