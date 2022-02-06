using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaterialController : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    private MeshRenderer rend;
    private Material activeMat;
    private Color stateColor;
    private float flashCount, flashTime = .25f;
    private bool isFlashing;

    private void OnEnable()
    {
        if(rend == null)
        {
            rend = GetComponent<MeshRenderer>();
        }
        activeMat = new Material(rend.material);
        rend.material = activeMat;
    }

    public void GoIdleMat()
    {
        stateColor = colors[0];
        if(!isFlashing)
        {
            activeMat.color = stateColor;
        }
    }

    public void GoAggroMat()
    {
        stateColor = colors[1];
        if (!isFlashing)
        {
            activeMat.color = stateColor;
        }
    }

    public void GoWindUpMat()
    {
        stateColor = colors[2];
        if (!isFlashing)
        {
            activeMat.color = stateColor;
        }
    }

    public void GoAttackMat()
    {
        stateColor = colors[3];
        if (!isFlashing)
        {
            activeMat.color = stateColor;
        }
    }

    private void Update()
    {
        if(isFlashing)
        {
            flashCount += Time.deltaTime;
            if(flashCount >= flashTime)
            {
                ToggleFlash(false);
            }
        }
    }

    public void ToggleFlash(bool start)
    {
        Debug.Log("flashing: " + start);
        if (start)
        {
            flashCount = 0;
            activeMat.color = Color.white;
        }
        else
        {
            activeMat.color = Color.white;
        }
        isFlashing = start;
    }

}
