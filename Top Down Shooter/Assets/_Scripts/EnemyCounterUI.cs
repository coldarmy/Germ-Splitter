using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounterUI : MonoBehaviour
{
    public static EnemyCounterUI instance;

    private TextMeshProUGUI displayText;

    private void OnEnable()
    {
        displayText = GetComponent<TextMeshProUGUI>();
       if(instance == null)
        {
            instance = this;
        }
    }

    public void UpdateCount(int num)
    {
        displayText.text = "Enemies: " +  num;
    }
}
