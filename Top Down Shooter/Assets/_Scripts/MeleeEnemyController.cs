using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    [SerializeField] private float idleTime, wanderTime;
    [SerializeField] private float wanderDistance, aggroRange, aggroLeashRange;
    [SerializeField] private float wanderSpeed, chaseSpeed, pathfindDistance;
    private Transform player;
    private Rigidbody rb;
    private float count;
    private int curAttacks;
    private Vector3 target;
    private List<Vector3> possibleDirections;
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
        possibleDirections = new List<Vector3>();
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
        /* float ranX = Random.Range(-wanderDistance, wanderDistance);
         float ranZ = Random.Range(-wanderDistance, wanderDistance);
         target = transform.position;
         target.x += ranX;
         target.z += ranZ;*/
        // Debug.DrawLine(transform.position, target, Color.red, 5f);
        target = RandomPathfind(5);
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

        GetComponent<EnemyHP>().TakeSuicideDamage();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            DoKamikazeAttack(collision.gameObject.GetComponent<PlayerController>());
        }
    }

    private void GetAllPossibleDirections()
    {
        possibleDirections.Clear();
        for(int i = 0; i < 16; i++)
        {
            float angleOfRayCast = i * 360 / 16;
            Vector3 dir = new Vector3(Mathf.Cos(angleOfRayCast * Mathf.Deg2Rad), 0, Mathf.Sin(angleOfRayCast * Mathf.Deg2Rad));
            Ray r = new Ray(transform.position, dir);
            //layermask mask - layermask.getmask("walls");
            if(!Physics.Raycast(r, out RaycastHit inf, pathfindDistance))
            {

                Debug.DrawLine(transform.position, transform.position + dir * pathfindDistance, Color.green, 1f);
                possibleDirections.Add(dir * pathfindDistance);
            }
            else
            {
                Debug.Log(inf.transform.tag);
                if(inf.distance >= pathfindDistance * .5f)
                {
                    Debug.DrawLine(transform.position, inf.point, Color.red, 1f);
                    possibleDirections.Add(dir * inf.distance * Random.Range(.5f, .75f));
                }
            }
        }
    }

  /*  private Vector3 PathfindWithObjective(Vector3 objective, bool towards, int random)
    {
        GetAllPossibleDirections();
        if(!towards)
        {
            possibleDirections.Sort((Vector3 a, Vector3 b) =>
            {
                if (a == b) return 0;
                return (int)(Vector3.Distance(a.normalized, objective.normalized) * 100f - Vector3.Distance(b.normalized, objective.normalized) * 100f);
            });
        }
        else
        {
            possibleDirections.Sort((Vector3 a, Vector3 b) =>
            {
                if (a == b) return 0;
                return (int)(Vector3.Distance(b.normalized, objective.normalized) * 100f - Vector3.Distance(a.normalized, objective.normalized) * 100f);
            });
        }
        if(possibleDirections.Count == 0)
        {
            return transform.position;
        }
        return transform.position + possibleDirections[Random.Range(0, Mathf.Min(possibleDirections.Count, random))];
    }*/

    private Vector3 RandomPathfind(int random)
    {
        GetAllPossibleDirections();
        possibleDirections.Sort((Vector3 a, Vector3 b) =>
        {
            if (a == b) return 0;
            return (int)(b.magnitude * 100f - a.magnitude * 100f);
        });
        Vector3 dir = possibleDirections[Random.Range(0, Mathf.Min(possibleDirections.Count, random))];
        //Debug.DrawLine(transform.position, transform.position + dir, Color.yellow, 1f);
        return transform.position + dir;
    }


  

}
