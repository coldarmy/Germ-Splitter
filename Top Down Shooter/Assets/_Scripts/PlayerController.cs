using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerController : MonoBehaviour
{
    public MMFeedbacks HitFeedback;
    public static PlayerController instance;
    public delegate void PlayerEvent(int damage);
    public static PlayerEvent OnPlayerTakesDamage;
    [SerializeField] private bool invulnerableDebug;
    [SerializeField] private int startingHP;
    [SerializeField] private Material standardMat, invulMat;
    private MeshRenderer rend;
    private int curHP;
    private bool invul;
    private float invulTime = .5f, invulCount;
    

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
        curHP = startingHP;
        rend = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(invul)
        {
            invulCount += Time.deltaTime;
            if(invulCount >= invulTime)
            {
                ToggleInvul(false);
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        
        curHP -= dmg;
        OnPlayerTakesDamage?.Invoke(curHP);
        HitFeedback?.PlayFeedbacks();
        if (curHP <= 0)
        {
            if (!invulnerableDebug)
            {
                Die();
            }
            
        }
        else
        {
            ToggleInvul(true);
        }
    }

    public void AddHealth()
    {
        curHP += 1;
        if(curHP > startingHP)
        {
            curHP = startingHP;
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void ToggleInvul(bool goingInvul)
    {
        if(goingInvul)
        {
            rend.material = invulMat;
            invulCount = 0;
        }
        else
        {
            rend.material = standardMat;
        }
        invul = goingInvul;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
        if (collision.gameObject.CompareTag("LevelBounds"))
        {
           // TakeDamage(startingHP);
        }
    }

    
}
