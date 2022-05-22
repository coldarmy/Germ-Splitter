using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementMelee : EnemyMovement
{
    private void DoKamikazeAttack(PlayerController p)
    {
        // Destroy(this.gameObject);

        GetComponent<EnemyHP>().TakeSuicideDamage();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DoKamikazeAttack(collision.gameObject.GetComponent<PlayerController>());
        }
    }

}
