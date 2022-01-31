﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class GunController : MonoBehaviour
{

    public BulletData standardBullet, specialBullet;
    public MMFeedbacks ShootFeedback;
    [SerializeField] private Rigidbody rb;
    private float bulletOffset = .65f;
    private float shootCD, playerKB; // get these values from the bullet
    private float cooldown;
    private LineRenderer lr;    
    
    private void OnEnable()
    {
        lr = GetComponent<LineRenderer>();
        AssignBulletValues(standardBullet.cooldown, standardBullet.playerKB);
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

    public void ShootGun(Vector3 dir)
    {
        BulletSpawner.instance.SpawnBullet(standardBullet, dir, this.transform.position, bulletOffset);
        ShootFeedback?.PlayFeedbacks();
        cooldown = shootCD;
    }

    public void ShootSpecial(Vector3 dir)
    {
        Debug.Log("shooting special");
        BulletSpawner.instance.SpawnBullet(specialBullet, dir, this.transform.position, bulletOffset);
        ShootFeedback?.PlayFeedbacks();
        cooldown = shootCD;
    }

    public bool CanShoot()
    {
        return cooldown <= 0;
    }


}
