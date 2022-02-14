﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float idleTime, wanderTime, attackWindUpTime, attackRate, attackCoolDown;
    [SerializeField] private float wanderDistance, aggroRange, aggroLeashRange, attackRange;
    [SerializeField] private float wanderSpeed, chaseSpeed;
    [SerializeField] private int numAttacks;
    [SerializeField] private BulletData myBullet;
    [SerializeField] private bool _MoveAndShoot;
    private float bulletOffset = 1f;
    private bool onAttackCoolDown;
    private Transform player;
    private Rigidbody rb;
    private float count;
    private int curAttacks;
    private Vector3 target;
    private EnemyMaterialController matController;
    [SerializeField] private enum EnemyState
    {
        Idle,
        Wander,
        Chase,
        WindUp,
        Attack,
        CoolDown
    }

    [SerializeField] private EnemyState state;

    private void OnEnable()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }        
        if(matController == null)
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
        switch(state)
        {
            case EnemyState.Idle:
                {
                    HandleIdle();
                    break;
                }
            case EnemyState.Wander:
                {
                    HandleWander();
                    break;
                }
            case EnemyState.Chase:
                {
                    HandleChase();
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

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(state)
        {
            case EnemyState.Wander:
                GoToPosition(target);
                break;
            case EnemyState.Chase:
                FollowPlayer();
                break;
            case EnemyState.WindUp:
                if(_MoveAndShoot)
                {
                    FollowPlayer();
                }
                break;
            case EnemyState.Attack:
                if (_MoveAndShoot)
                {
                    FollowPlayer();
                }
                break;
            case EnemyState.CoolDown:
                if (_MoveAndShoot)
                {
                    FollowPlayer();
                }
                break;
        }

        
    }

    private void HandleIdle()
    {
        if(onAttackCoolDown)
        {
            if(count >= attackCoolDown)
            {
                onAttackCoolDown = false;
                count = 0;
            }
            return;
        }
        CheckAggro(false);
        if (count >= idleTime)
        {
            GoToWander();
        }
    }

    private void HandleWander()
    {
        CheckAggro(false);
        if (Vector3.Distance(transform.position, target) <= 1f)
        {
            GoToIdle();

        }
    }

    private void HandleChase()
    {
        CheckAttack();
        CheckAggro(true);
    }

    private void HandleWindUp()
    {
        if(count >= attackWindUpTime)
        {
            GoToAttack();
        }
    }

    private void HandleAttacking()
    {
        if(count >= attackRate)
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

    private void GoToWander()
    {
        float ranX = Random.Range(-wanderDistance, wanderDistance);
        float ranZ = Random.Range(-wanderDistance, wanderDistance);
        target = transform.position;
        target.x += ranX;
        target.z += ranZ;
        Debug.DrawLine(transform.position, target, Color.red, 5f);
        count = 0;
        state = EnemyState.Wander;
    }

    private void GoToChase()
    {
        matController.GoAggroMat();
        // player = PlayerController.instance.transform;
        state = EnemyState.Chase;
        count = 0;

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
        transform.LookAt(player);
        SpawnBullet();

        //spawn bullet
        curAttacks++;
        if(numAttacks == curAttacks)
        {
            onAttackCoolDown = true;
            if(_MoveAndShoot)
            {
                GoToChase();
            }
            else
            {
                GoToIdle();
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



    private void FollowPlayer()
    {
        transform.LookAt(player);
        rb.MovePosition( transform.position + (transform.forward * chaseSpeed * Time.fixedDeltaTime));
    }

    private void GoToPosition(Vector3 targetPos)
    {
        Vector3 relativePos = targetPos - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
        rb.MovePosition(transform.position + (transform.forward * wanderSpeed * Time.fixedDeltaTime));
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

    private void CheckAggro(bool chasing)
    {
        if(state == EnemyState.WindUp || state == EnemyState.CoolDown || state == EnemyState.Attack)
        {
            return;
        }
        if(chasing)
        {
            

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, aggroLeashRange);
            if(hitColliders.Length > 0)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].CompareTag("Player"))
                    {
                       // Debug.Log("player still in range: " + Vector3.Distance(transform.position, hitColliders[i].transform.position));
                        GoToChase();
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
            if(hitColliders.Length > 0)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].CompareTag("Player"))
                    {
                        GoToChase();
                        return;
                    }
                }
            }         
        }
        
    }
}