using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int maxEnemies;

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
        Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Break();
        }
    }
}
