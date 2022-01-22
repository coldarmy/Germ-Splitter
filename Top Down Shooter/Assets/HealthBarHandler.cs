using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    public bool assigned;
    [SerializeField] private Image redHealth, whiteHealth;
    [SerializeField] private Vector3 offset;
    [SerializeField]private Transform target;
    private bool showing;

    public void ShowHealthBar(bool show)
    {
        showing = show;
        gameObject.SetActive(showing);
    }

    public void UpdateHPValue(float value)
    {
        redHealth.fillAmount = value;
    }

    private void Update()
    {
        if(showing)
        {
            if(target != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
            }
            
            whiteHealth.fillAmount = Mathf.Lerp(whiteHealth.fillAmount, redHealth.fillAmount, Time.deltaTime * 4f);
            if(whiteHealth.fillAmount <= 0.01f)
            {
                ShowHealthBar(false);
            }
        }
    }

    public void AssignHPBar(Transform t)
    {
        target = t;
        showing = false;
        whiteHealth.fillAmount = 1f;
        redHealth.fillAmount = 1f;
        assigned = true;
    }

    public void UnassignHPBar()
    {
        assigned = false;
        //target = null;
    }



}
