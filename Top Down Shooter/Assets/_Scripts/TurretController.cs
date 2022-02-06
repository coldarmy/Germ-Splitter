using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float attackWindUpTime, attackRate, attackCoolDown;
    [SerializeField] private float aggroRange, aggroLeashRange, attackRange;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private int numAttacks;
    [SerializeField] private BulletData myBullet;
    private float bulletOffset = 1f;
    [SerializeField]private bool onAttackCoolDown;
    private Transform player;
    private Rigidbody rb;
    [SerializeField]private float count;
    private int curAttacks;
    private EnemyMaterialController matController;
    [SerializeField]
    private enum EnemyState
    {
        Idle,
        Tracking,
        WindUp,
        Attack,
        CoolDown
    }

    [SerializeField] private EnemyState state;

    private void OnEnable()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (matController == null)
        {
            matController = GetComponent<EnemyMaterialController>();
        }
    }
    private void Start()
    {
        player = PlayerController.instance.transform;
        GoToIdle();

    }

    private void Update()
    {
        count += Time.deltaTime;
        if (onAttackCoolDown)
        {
            if (count >= attackCoolDown)
            {
                onAttackCoolDown = false;
                count = 0;
            }
        }
        switch (state)
        {
            case EnemyState.Idle:
                {
                    HandleIdle();
                    break;
                }
            case EnemyState.Tracking:
                {
                    HandleTracking();
                    break;
                }
            case EnemyState.WindUp:
                {
                    HandleWindUp();
                    break;
                }
            case EnemyState.Attack:
                {
                    HandleAttacking();
                    break;
                }
        }
    }

    private void HandleIdle()
    {
        CheckAggro(false);
        //rotate around
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void HandleTracking()
    {
        transform.LookAt(player);
        CheckAggro(true);
        if (!onAttackCoolDown)
        {
            CheckAttack();
        }
    }

    private void HandleWindUp()
    {
        transform.LookAt(player);
        if (count >= attackWindUpTime)
        {
            GoToAttack();
        }
    }

    private void HandleAttacking()
    {
        transform.LookAt(player);
        if (count >= attackRate && !onAttackCoolDown)
        {
            DoAttack();
        }
    }


    private void GoToIdle()
    {
        count = 0;
        state = EnemyState.Idle;
        matController.GoIdleMat();
    }

    private void GoToTracking()
    {
        matController.GoAggroMat();
        count = 0;
        state = EnemyState.Tracking;
    }

    

    private void GoToWindUp()
    {
        matController.GoWindUpMat();
        count = 0;
        curAttacks = 0;
        state = EnemyState.WindUp;
    }

    private void GoToAttack()
    {
        matController.GoAttackMat();
        count = 0;
        curAttacks = 0;
        state = EnemyState.Attack;
    }


    private void DoAttack()
    {        
        SpawnBullet();

        //spawn bullet
        curAttacks++;
        if (numAttacks == curAttacks)
        {
            onAttackCoolDown = true;
            if(InAttackRange())
            {
                GoToIdle();
            }
            else
            {
                GoToTracking();
            }
        }
        else
        {
            count = 0;
        }

    }

    private void SpawnBullet()
    {
        BulletSpawner.instance.SpawnBullet(myBullet, transform.forward, transform.position, bulletOffset);
    }         

    private void CheckAttack()
    {
        // Debug.Log("checking attack");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].CompareTag("Player"))
                {
                    GoToWindUp();
                    return;
                }
            }
        }
    }

    private bool InAttackRange()
    {
        return (Vector3.Distance(transform.position, player.position) <= aggroLeashRange);
    }

    private void CheckAggro(bool chasing)
    {
        if (state == EnemyState.WindUp || state == EnemyState.CoolDown || state == EnemyState.Attack)
        {
            return;
        }
        if (chasing)
        {


            Collider[] hitColliders = Physics.OverlapSphere(transform.position, aggroLeashRange);
            if (hitColliders.Length > 0)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].CompareTag("Player"))
                    {
                        // Debug.Log("player still in range: " + Vector3.Distance(transform.position, hitColliders[i].transform.position));
                        //GoToTracking();
                        return;
                    }
                }
                //  Debug.Log("play too far away, going back to idle");
                GoToIdle();
            }
            else
            {
                //Debug.Log("play too far away, going back to idle");
                GoToIdle();
            }

        }
        else
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, aggroRange);
            if (hitColliders.Length > 0)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].CompareTag("Player"))
                    {
                        GoToTracking();
                        return;
                    }
                }
            }
        }

    }
}
