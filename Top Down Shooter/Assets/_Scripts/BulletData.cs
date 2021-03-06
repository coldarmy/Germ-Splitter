using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BulletData", menuName = "Shooter/Bullets", order = 1)]
public class BulletData : ScriptableObject
{
    [SerializeField] private string name;
    public float cooldown, hitKB, lifeTime, moveSpeed, energyCost, stunTime;
    public bool backwards;
    public GameObject bulletObject;
    public int damage, poolSize;
    
}
