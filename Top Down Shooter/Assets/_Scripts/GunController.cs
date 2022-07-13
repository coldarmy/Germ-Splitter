using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class GunController : MonoBehaviour
{

    public BulletData standardBullet, specialBullet1, specialBullet2;
    public MMFeedbacks ShootFeedback;
    [SerializeField] private Rigidbody rb;
    private float forwardBulletOffset = .65f;
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

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }        
    }

    public void ShootGun(Vector3 dir)
    {
        BulletSpawner.instance.SpawnBullet(standardBullet, dir, this.transform.position, forwardBulletOffset);
        ShootFeedback?.PlayFeedbacks();
        cooldown = shootCD;
    }

    public void ShootSpecial1(Vector3 dir)
    {
        if(EnergyController.CanShoot(specialBullet1.energyCost))
        {
            if(specialBullet1.backwards)
            {
                BulletSpawner.instance.SpawnBullet(specialBullet1, dir, this.transform.position, -forwardBulletOffset);
            }
            else
            {
                BulletSpawner.instance.SpawnBullet(specialBullet1, dir, this.transform.position, forwardBulletOffset);
            }
            
            ShootFeedback?.PlayFeedbacks();
            cooldown = shootCD;
            EnergyController.ChangeEnergy(-specialBullet1.energyCost);
        }        
    }

    public void ShootSpecial2(Vector3 dir)
    {
        if (EnergyController.CanShoot(specialBullet2.energyCost))
        {
            if(specialBullet2.backwards)
            {
                BulletSpawner.instance.SpawnBullet(specialBullet2, dir, this.transform.position, -forwardBulletOffset);
            }
            else
            {
                BulletSpawner.instance.SpawnBullet(specialBullet2, dir, this.transform.position, forwardBulletOffset);
            }
            
            ShootFeedback?.PlayFeedbacks();
            cooldown = shootCD;
            EnergyController.ChangeEnergy(-specialBullet2.energyCost);
        }
    }

    public float GetSpecialWeaponEenergy(int num)
    {
        if(num == 1)
        {
            return specialBullet1.energyCost;
        }
        if(num == 2)
        {
            return specialBullet2.energyCost;
        }
        else
        {
            Debug.Log("not valid special weapon number");
            return 0f;
        }
    }


    public bool CanShoot()
    {
        return cooldown <= 0;
    }


}
