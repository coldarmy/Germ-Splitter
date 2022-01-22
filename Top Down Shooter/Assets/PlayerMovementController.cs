using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
   [SerializeField] private float moveSpeed, rotSpeed;
    private Rigidbody rb;
    private float distanceToCam;
    private Camera cam;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        distanceToCam = Vector3.Distance(cam.transform.position, transform.position);
    }

    private void FixedUpdate()
    {
        Move();
       //LookAtMouse();
    }

    private void Update()
    {
        LookAtMouse();
    }




    private void Move()
    {
        Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.MovePosition(transform.position +(Movement * moveSpeed * Time.fixedDeltaTime));
    }

    private void LookAtMouse()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = distanceToCam; // select distance = 10 units from the Bcamera
        Vector3 mouseTarget = cam.ScreenToWorldPoint(mousePos);
        mouseTarget.y = transform.position.y;
        Debug.DrawLine(transform.position, mouseTarget, Color.blue, 1f);
        Quaternion targetRot = Quaternion.LookRotation(mouseTarget - rb.position, transform.up);
          Quaternion movementRot = Quaternion.Lerp(rb.rotation, targetRot, rotSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(movementRot);
       
    }
}
