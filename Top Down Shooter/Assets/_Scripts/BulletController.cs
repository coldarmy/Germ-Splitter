using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public BulletData myBulletData;
    private float count, lifeTime, moveSpeed;
    private Rigidbody rb;
    Vector3 moveDir;

    protected virtual void OnEnable()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        lifeTime = myBulletData.lifeTime;
        count = 0;
        moveDir = transform.forward;
        moveSpeed = myBulletData.moveSpeed;
        //Debug.Break();
       // rb.AddForce(dir * myBulletData.FireForce);

    }

    protected virtual void Update()
    {
        count += Time.deltaTime;
        if( count >= lifeTime)
        {
            TurnOffBullet();
        }
        MoveForward();
    }

    private void MoveForward()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("LevelBounds"))
        {
            TurnOffBullet();
        }        
    }


    public virtual void TurnOffBullet()
    {
        // rb.velocity = Vector3.zero;
        // rb.angularVelocity = Vector3.zero;
        //Debug.Log("turning off bullet");
        this.gameObject.SetActive(false);
    }
}
