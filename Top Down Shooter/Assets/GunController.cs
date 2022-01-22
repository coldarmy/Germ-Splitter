using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class GunController : MonoBehaviour
{

    public BulletData myBulletType;
    public MMFeedbacks ShootFeedback;
    [SerializeField] private Rigidbody rb;
    private float bulletOffset = .5f;
    private float shootCD, playerKB; // get these values from the bullet
    private float cooldown;
    private LineRenderer lr;
    
    
    private void OnEnable()
    {
        lr = GetComponent<LineRenderer>();
        AssignBulletValues(myBulletType.cooldown, myBulletType.playerKB);
        cooldown = 0;

    }

    

    private void AssignBulletValues(float cd, float kickBack)
    {
        shootCD = cd;
        playerKB = kickBack;
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        
    }

   /* private void LateUpdate()
    {
        
        Vector3 startPos = transform.position + transform.up * 1f;
        Vector3 endPos = startPos;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(startPos, transform.up, out hit, 500f))
        {
            endPos = hit.point;
            endPos.y = startPos.y;
        }
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
    }*/

    public void ShootGun(Vector3 dir)
    {
        //Debug.Log("shoot");
        BulletSpawner.instance.SpawnBullet(myBulletType, dir, this.transform.position, bulletOffset);
        //  KickBack();
        ShootFeedback?.PlayFeedbacks();
        cooldown = shootCD;
    }

    public bool CanShoot()
    {
        return cooldown <= 0;
    }
    
    private void KickBack()
    {
        Vector3 dir = -(transform.up.normalized);
        rb.AddForce(dir * playerKB);
    }

}
