using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlaiveController : MonoBehaviour
{
    [SerializeField] private float range, moveSpeed;
    [SerializeField] private int startingBounces;
    [SerializeField] private int[] damages;
    [SerializeField] private GameObject particles;
    private int curBounce;
    private Transform target;
    private List<Transform> previousTargets, possibleTargets;



    public void Spawn(Transform firstHitObject)
    {
        if(previousTargets == null)
        {
            previousTargets = new List<Transform>();
        }
        else
        {
            previousTargets.Clear();
        }
        if(possibleTargets == null)
        {
            possibleTargets = new List<Transform>();
        }

        GameObject exp = LeanPool.Spawn(particles);
        exp.transform.position = this.transform.position;
        previousTargets.Add(firstHitObject);
        curBounce = 0;
        target = FindTarget();
        if(target == null)
        {
            Debug.Log("no nearby targets");
            this.gameObject.SetActive(false);
        }
        else
        {
            Vector3 dir = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, target.transform.position) <= .1f)
        {
            AttackTarget();
        }
                
    }

    private void AttackTarget()
    {
        GameObject exp = LeanPool.Spawn(particles);
        exp.transform.position = this.transform.position;
        
        target.GetComponent<EnemyHP>().TakeDamage(damages[curBounce]);
        //also spawn explosion effect?
        curBounce++;
        if(curBounce < startingBounces)
        {
            GoToNextTarget(target);
        }
        else
        {
            this.gameObject.SetActive(false);
        }

    }

    private Transform FindTarget()
    {
        possibleTargets.Clear();
        Collider[] cols;
        cols = Physics.OverlapSphere(transform.position, range);
        if(cols.Length > 0)
        {
            foreach(Collider c in cols)
            {
                if(c.gameObject.CompareTag("Enemy"))
                {
                    if(!previousTargets.Contains(c.transform))
                    {
                        possibleTargets.Add(c.transform);
                    }
                }
            }
        }

        if(possibleTargets.Count == 0)
        {
            return null;
        }

        if(possibleTargets.Count == 1)
        {
            return possibleTargets[0];
        }
        else
        {
            return ClosestTarget(possibleTargets);
        }
    }

    private Transform ClosestTarget(List<Transform> targets)
    {
        int closest = 0;
        float closestDistance = Vector3.Distance(transform.position, targets[0].position);
        for (int i = 1; i < targets.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, targets[i].position);
            if(distance < closestDistance)
            {
                closest = i;
            }
        }
        return targets[closest];
    }
    
    private void GoToNextTarget(Transform curTarget)
    {
        previousTargets.Add(curTarget);
        target = FindTarget();
        if (target == null)
        {
            Debug.Log("no nearby targets");
            this.gameObject.SetActive(false);
        }
        else
        {
            Vector3 dir = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }



}
