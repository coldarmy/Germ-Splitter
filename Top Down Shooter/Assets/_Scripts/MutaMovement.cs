using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutaMovement : MonoBehaviour
{
    [SerializeField] private ControlType controlState;
    [SerializeField] private float maxMoveSpeed, rotSpeed, accelRate, decelRate;
    private float distanceToCam;
    [SerializeField] private float curMoveSpeed, finalMoveSpeed;
    [SerializeField]private bool moving, boosting;
    [SerializeField] public bool usingBoost;
    private Camera cam;
    private Vector3 moveDir;
    private GunController myGun;
    private Rigidbody _rb;
    private Quaternion oldLookRot;
    private Vector3 mousePos, mouseTarget, shootDir;
    private PlayerDashController _dashController;

    [SerializeField] private enum ControlType
    {
        Mouse,
        Controller
    }

    private void OnEnable()
    {
        _dashController = GetComponent<PlayerDashController>();
        _rb = GetComponent<Rigidbody>();
        myGun = GetComponent<GunController>();
        cam = Camera.main;
        distanceToCam = Vector3.Distance(cam.transform.position, transform.position);
        moveDir = transform.forward;
        if(controlState == ControlType.Mouse)
        {
           // moving = true;
        }
    }


    private void Update()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            moving = true;
        }
        if (Input.GetButtonUp("Vertical"))
        {
            moving = false;
        }

        if (Input.GetMouseButton(0) && myGun.CanShoot())
        {
            shootDir = (mouseTarget - transform.position).normalized;
            myGun.ShootGun(shootDir);
        }
        if (Input.GetMouseButton(1) && myGun.CanShoot())
        {
            myGun.ShootSpecial1(moveDir);
        }

        if (Input.GetMouseButton(2) && myGun.CanShoot())
        {
            //Debug.Log("trying to shoot mine");
            myGun.ShootSpecial2(moveDir);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
        /*    if(usingBoost)
            {
                TryToBoost();
            }
            else
            {
                if (_dashController.CanDash())
                {
                    StartDash();
                }
            }*/           

        }
    }

    private void FixedUpdate()
    {
        switch (controlState)
        {
            case ControlType.Controller:
                HandleControllerMovement();
                break;
            case ControlType.Mouse:
                HandleMouseMovement();
                break;
        }
    }


    private void HandleControllerMovement()
    {

    }

    private void HandleMouseMovement()
    {
       /* if(!_dashController.dashing)
        {
           
            ChangeAcceleration(!moving, _dashController.dashing);
            
        }*/
      
        ChangeAcceleration(!moving, _dashController.dashing);
        LookAtMouse();
        MoveForward();

    }
   

    private void ChangeAcceleration(bool slowing, bool boosting)
    {
        if(boosting)
        {
            curMoveSpeed += accelRate * Time.deltaTime;
        }
        else
        {
            if (slowing)
            {
                curMoveSpeed -= decelRate * Time.deltaTime;
            }
            else
            {
                curMoveSpeed += accelRate * Time.deltaTime;
            }
        }

       
        curMoveSpeed = Mathf.Clamp(curMoveSpeed, 0, maxMoveSpeed);
    }    

    private void MoveForward()
    {
        finalMoveSpeed = curMoveSpeed * _dashController.curBoostSpeed;
        _rb.velocity = moveDir * finalMoveSpeed;// curMoveSpeed * _dashController.curBoostSpeed;// * Time.fixedDeltaTime;
    }

    private void LookAtMouse()
    {
        mousePos = Input.mousePosition;
        distanceToCam = Vector3.Distance(cam.transform.position, transform.position);
        mousePos.z = distanceToCam; // select distance = 10 units from the Bcamera
        //Vector3 mouseTarget = cam.ScreenToWorldPoint(mousePos);
        //mouseTarget.y = transform.position.y;
        mouseTarget = GetMouseTarget(mousePos);
        //Debug.DrawLine(transform.position, mouseTarget, Color.blue, 1f);
        Quaternion targetRot = Quaternion.LookRotation(mouseTarget - transform.position, transform.up);
        oldLookRot = transform.rotation;
        transform.LookAt(mouseTarget);
        transform.rotation = Quaternion.Lerp(oldLookRot, transform.rotation, rotSpeed * Time.fixedDeltaTime);
        moveDir = transform.forward;
    }

    public Vector3 GetMouseTarget(Vector3 mousePosition)
    {
        Vector3 pos = cam.ScreenToWorldPoint(mousePosition);
        pos.y = transform.position.y;
        return pos;
    }

  /*  public void StartDash()
    {
       // Debug.Log("dashing");
        _dashController.StartDash();
        curMoveSpeed = _dashController.dashSpeed;
    }*/

   /* private void TryToBoost()
    {
        if(_dashController.CanBoost())
        {
            _dashController.SpendBoostEnergy();
            curMoveSpeed = _dashController.boostSpeed;
        }
        else
        {
            //curMoveSpeed = 
        }
    }*/
}
