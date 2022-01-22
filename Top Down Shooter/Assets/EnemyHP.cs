using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void EnemyEvent();
    public static EnemyEvent OnEnemyDeath;
    public MMFeedbacks HitFeedback, DeathFeedback;
    [SerializeField] private int startingHP;
    private int curHP;
    private EnemyMaterialController matController;
    [SerializeField]private HealthBarHandler myHP;
    private void OnEnable()
    {
        curHP = startingHP;
        matController = GetComponent<EnemyMaterialController>();
        myHP = EnemyUIController.instance.GetUnusedHPBar(this.transform);
    }

    public void TakeDamage(int dmg)
    {
        curHP -= dmg;
        myHP.ShowHealthBar(true);
        myHP.UpdateHPValue((float)curHP / (float)startingHP);
        
        //ToggleFlash(true);
        matController.ToggleFlash(true);
        HitFeedback?.PlayFeedbacks();
        if (curHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        DeathFeedback?.PlayFeedbacks();
        myHP.UnassignHPBar();
        OnEnemyDeath?.Invoke();
        this.gameObject.SetActive(false);
    }

    public void AssignHPBar(HealthBarHandler newHP)
    {
        myHP = newHP;
    }

   

}
