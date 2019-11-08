using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairBehaviour : MonoBehaviour
{
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    public void Start()
    {
        Cursor.visible = false;
    }

    public void Update()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -camera.transform.position.z;
        var mouseWorldPos = camera.ScreenToWorldPoint(mouseScreenPos);

        transform.position = mouseWorldPos;
    }
}
