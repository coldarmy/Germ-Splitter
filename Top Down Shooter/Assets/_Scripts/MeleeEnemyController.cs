using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    [SerializeField] private float idleTime, wanderTime;
    [SerializeField] private float wanderDistance, aggroRange, aggroLeashRange;
    [SerializeField] private float wanderSpeed, chaseSpeed;
    private Transform player;
    private Rigidbody rb;
    private float count;
    private int curAttacks;
    private Vector3 target;
    private EnemyMaterialController matController;
    [SerializeField]
    private enum EnemyState
    {
        Idle,
        Wander,
        Chase,
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
        switch (state)
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
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case EnemyState.Wander:
                GoToPosition(target);
                break;
            case EnemyState.Chase:
                FollowPlayer();
                break;
        }


    }

    private void HandleIdle()
    {
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
        CheckAggro(true);
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
    private void FollowPlayer()
    {
        transform.LookAt(player);
        rb.MovePosition(transform.position + (transform.forward * chaseSpeed * Time.fixedDeltaTime));
    }

    private void GoToPosition(Vector3 targetPos)
    {
        Vector3 relativePos = targetPos - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
        rb.MovePosition(transform.position + (transform.forward * wanderSpeed * Time.fixedDeltaTime));
    }


    private void CheckAggro(bool chasing)
    {
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
            if (hitColliders.Length > 0)
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

    private void DoKamikazeAttack(PlayerController p)
    {
        // Destroy(this.gameObject);
        GetComponent<EnemyHP>().TakeDamage(1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            DoKamikazeAttack(collision.gameObject.GetComponent<PlayerController>());
        }
    }

}
