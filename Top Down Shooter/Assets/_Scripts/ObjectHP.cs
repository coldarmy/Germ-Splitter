using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHP : EnemyHP
{
    public override void Die()
    {
        LandmineController lc = GetComponent<LandmineController>();
        if(lc != null)
        {
            lc.GetHitByBullet();
        }

    }
}
