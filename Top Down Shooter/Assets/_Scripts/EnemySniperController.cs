using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniperController : EnemyMovement
{
    private LineRenderer lr;
    private bool displayingLaser;
    // Start is called before the first frame update
    private void OnEnable()
    {
        base.OnEnable();
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        base.Update();
        if (displayingLaser)
        {
            DrawLaser();
        }
    }

    public override void GoToWindUp()
    {
        base.GoToWindUp();
        ToggleLaser(true);
    }

    public override void GoToChase()
    {
        base.GoToChase();
        ToggleLaser(false);
    }

    public override void GoToIdle()
    {
        base.GoToIdle();
        ToggleLaser(false);
    }

    public override void GoToWander()
    {
        base.GoToWander();
        ToggleLaser(false);
    }

    private void ToggleLaser(bool status)
    {
        lr.enabled = status;
        displayingLaser = status;
    }

    private void DrawLaser()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.forward * GetAttackRange());
    }
}
