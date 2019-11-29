using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void Start()
    {
        Cursor.visible = false;
    }

    public void FixedUpdate()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -cam.transform.position.z;
        var mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        transform.position = mouseWorldPos;
    }
}
