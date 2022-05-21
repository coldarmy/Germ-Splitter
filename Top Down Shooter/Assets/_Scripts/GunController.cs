using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class GunController : MonoBehaviour
{

    public BulletData standardBullet, specialBullet;
    public MMFeedbacks ShootFeedback;
    [SerializeField] private Rigidbody rb;
    private float bulletOffset = .65f;
    private float shootCD; // get these values from the bullet
    private float cooldown;
    private LineRenderer lr;
    private PlayerEnergyController EnergyController;
    
    private void OnEnable()
    {;
        lr = GetComponent<LineRenderer>();
        AssignBulletValues(standardBullet.cooldown);
        cooldown = 0;

    }

    private void Start()
    {
        EnergyController = PlayerController.instance.GetComponent<PlayerEnergyController>();
    }

    private void AssignBulletValues(float cd)
    {
        shootCD = cd;
         
    }

    private void Update()
    {
      /*  if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("pressing p");
           // ObjectPoolManager.instance.SpawnExplosion(transform.position);

        }*/

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
        if(EnergyController.CanShoot(specialBullet.energyCost))
        {
            Debug.Log("spawning special");
            BulletSpawner.instance.SpawnBullet(specialBullet, dir, this.transform.position, bulletOffset);
            ShootFeedback?.PlayFeedbacks();
            cooldown = shootCD;
            EnergyController.ChangeEnergy(-specialBullet.energyCost);
        }        
    }

    public bool CanShoot()
    {
        return cooldown <= 0;
    }


}
