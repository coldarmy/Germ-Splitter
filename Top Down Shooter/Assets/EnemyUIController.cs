using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIController : MonoBehaviour
{
    public static EnemyUIController instance;
    [SerializeField] private HealthBarHandler enemyHPBar;
    private HealthBarHandler[] hpBars;

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        hpBars = new HealthBarHandler[GameController.instance.maxEnemies];
        for(int i = 0; i < hpBars.Length; i++)
        {
            hpBars[i] = Instantiate(enemyHPBar.gameObject, this.transform).GetComponent<HealthBarHandler>();
        }
    }

    public HealthBarHandler GetUnusedHPBar( Transform t)
    {
        for(int i = 0; i < hpBars.Length; i++)
        {
            if(!hpBars[i].assigned)
            {
                hpBars[i].AssignHPBar(t);
                return hpBars[i];
            }
        }
        Debug.Log(" no unused health bars!");
        return null;
    }
}
