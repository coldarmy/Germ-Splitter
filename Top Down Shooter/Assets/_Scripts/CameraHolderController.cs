using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolderController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _camMoveSpeed;
    private Transform target;
    Vector3 newPos;

    private void Start()
    {
        target = PlayerController.instance.transform;
        newPos.x = target.position.x;
        newPos.y = transform.position.y;
        newPos.z = target.position.z;
        transform.position = newPos;

    }

    private void LateUpdate()
    {
        if(target != null)
        {
            MoveToNewTargetPos();
        }
    }

    private void MoveToNewTargetPos()
    {
        newPos.x = target.position.x;
        newPos.z = target.position.z;
        transform.position = Vector3.MoveTowards(transform.position, newPos, _camMoveSpeed * Time.deltaTime) ;
    }
}
