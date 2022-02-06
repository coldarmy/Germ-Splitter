using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private Transform crosshair;

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        crosshair.position = Input.mousePosition;
    }
}
