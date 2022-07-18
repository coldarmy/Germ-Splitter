using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void EnemyEvent(Vector3 pos);
    public static EnemyEvent OnEnemyDeath;
    public MMFeedbacks HitFeedback, DeathFeedback;
    [SerializeField] private int startingHP;
    [SerializeField] private bool displayHP = true;
    private int curHP;
    private EnemyMaterialController matController;
    [SerializeField]private HealthBarHandler myHP;
    private EnemyMovement myEnemyMovement;
    private bool dead;
    private void OnEnable()
    {
        curHP = startingHP;
        matController = GetComponent<EnemyMaterialController>();
        if(displayHP)
        {
            myHP = EnemyUIController.instance.GetUnusedHPBar(this.transform);
        }
        
        myEnemyMovement = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(int dmg, float stunTime)
    {
        curHP -= dmg;
        if(myHP !=null)
        {
            myHP.ShowHealthBar(true);
            myHP.UpdateHPValue((float)curHP / (float)startingHP);
        }
        
        if(stunTime > 0)
        {
            if(myEnemyMovement != null)
            {
                myEnemyMovement.GetStunned(stunTime);
            }
        }
        //ToggleFlash(true);
        if(matController != null)
        {
            matController.ToggleFlash(true);
        }
        
        HitFeedback?.PlayFeedbacks();
        if (curHP <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("explosion"))
        {
            ExplosionController ex = other.GetComponent<ExplosionController>();
            TakeDamage(ex.damage, ex.stunTime);
            //Debug.Log("touching explosion");
            if(GetComponent<MeleeEnemyController>() != null)
            {
                GetComponent<MeleeEnemyController>().GetStunned(.5f);
            }
        }
    }

    public virtual void Die()
    {
        if (dead) return;
        dead = true;
        DeathFeedback?.PlayFeedbacks();
        if(myHP != null)
        {
            myHP.UnassignHPBar();
        }
        if(displayHP)
        {
            OnEnemyDeath?.Invoke(transform.position);
        }
        
        this.gameObject.SetActive(false);
    }

    public void AssignHPBar(HealthBarHandler newHP)
    {
        if (myHP != null)
        {
            myHP = newHP;
        }
    }

    public void TakeSuicideDamage()
    {
        TakeDamage(startingHP, 0f);
    }

   

}
