using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutaMovement : MonoBehaviour
{
    [SerializeField] private ControlType controlState;
    [SerializeField] private float maxMoveSpeed, rotSpeed, accelRate, decelRate;
    private float distanceToCam, curMoveSpeed;
    private bool moving;
    private Camera cam;
    [SerializeField]private Vector3 moveDir;
    private GunController myGun;
    private Rigidbody _rb;
    [SerializeField] private enum ControlType
    {
        Mouse,
        Controller
    }

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        myGun = GetComponent<GunController>();
        cam = Camera.main;
        distanceToCam = Vector3.Distance(cam.transform.position, transform.position);
        moveDir = transform.forward;
        if(controlState == ControlType.Mouse)
        {
            moving = true;
        }
    }

    

   /* private void Update()
    {        
        switch(controlState)
        {
            case ControlType.Controller:
                HandleControllerMovement();
                break;
            case ControlType.Mouse:
                HandleMouseMovement();
                    break;
        }        
    }*/

    private void FixedUpdate()
    {
        LookAtMouse();
        if (Input.GetMouseButton(0) && myGun.CanShoot())
        {
            myGun.ShootGun(moveDir);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            moving = !moving;
        }
        ChangeAcceleration(!moving);
        MoveForward();
    }



    private void HandleControllerMovement()
    {

    }

    private void HandleMouseMovement()
    {
        LookAtMouse();

        if (Input.GetMouseButton(0) && myGun.CanShoot())
        {
            myGun.ShootGun(moveDir);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            moving = !moving;
        }
        ChangeAcceleration(!moving);
        MoveForward();
    }
   

    private void ChangeAcceleration(bool slowing)
    {
        if(slowing)
        {
            curMoveSpeed -= decelRate * Time.deltaTime;            
        }
        else
        {
            curMoveSpeed += accelRate * Time.deltaTime;
        }
        curMoveSpeed = Mathf.Clamp(curMoveSpeed, 0, maxMoveSpeed);
    }
    

    private void MoveForward()
    {

        //transform.position += transform.forward * curMoveSpeed * Time.deltaTime;
        // _rb.MovePosition(transform.position + (transform.forward * curMoveSpeed * Time.fixedDeltaTime));
        _rb.velocity = moveDir * curMoveSpeed;// * Time.fixedDeltaTime;
    }

    private void LookAtMouse()
    {
        var mousePos = Input.mousePosition;
        distanceToCam = Vector3.Distance(cam.transform.position, transform.position);
        mousePos.z = distanceToCam; // select distance = 10 units from the Bcamera
        Vector3 mouseTarget = cam.ScreenToWorldPoint(mousePos);
        mouseTarget.y = transform.position.y;
        Debug.DrawLine(transform.position, mouseTarget, Color.blue, 1f);
        Quaternion targetRot = Quaternion.LookRotation(mouseTarget - transform.position, transform.up);
        // Quaternion movementRot = Quaternion.Lerp(rb.rotation, targetRot, rotSpeed * Time.fixedDeltaTime);
        //rb.MoveRotation(movementRot);
       // moveDir = Vector3.Lerp(moveDir, targetRot.eulerAngles, rotSpeed * Time.deltaTime).normalized;
        transform.LookAt(mouseTarget);
        moveDir = transform.forward;
    }
}
