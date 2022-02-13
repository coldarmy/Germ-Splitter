using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandler : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerEnergyController _energyController;

    private void OnEnable()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _energyController = _playerController.GetComponent<PlayerEnergyController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("pickup"))
        {

            PickUpController p = collision.transform.GetComponentInParent<PickUpController>();
            if(p.fading)
            {
                p.fading = false;
                StartCoroutine(MovePickUpToPlayer(p));
            }            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("touching: " + other.name);
        if(other.CompareTag("pickup"))
        {

            PickUpController p = other.transform.GetComponentInParent<PickUpController>();
            if (p.fading)
            {
                p.fading = false;
                StartCoroutine(MovePickUpToPlayer(p));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("pickup"))
        {
            PickUpController p = other.transform.GetComponentInParent<PickUpController>();
            if (p.fading)
            {
                p.fading = false;
                StartCoroutine(MovePickUpToPlayer(p));
            }
        }
    }

    private IEnumerator MovePickUpToPlayer(PickUpController p)
    {
        float t = 0;
        while (t < 1)
        {
            p.transform.position = Vector3.Lerp(p.transform.position, transform.position, t);
            t += Time.deltaTime * 4f;
            yield return null;
        }
        if (p._type == PickUpController.pickupType.health)
        {
            _playerController.AddHealth();
        }
        if(p._type == PickUpController.pickupType.energy)
        {
            //get energy
            _energyController.ChangeEnergy(p.energyAmt);
        }
        p.gameObject.SetActive(false);
    }
}
