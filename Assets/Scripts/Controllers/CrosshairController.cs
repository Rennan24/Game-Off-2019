using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class CrosshairController : MonoBehaviour
{
    private Camera cam;

    public float Radius;
    public float smoothing = 1;

    [SerializeField]
    private InputController input;

    private Vector2 newDir;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        Cursor.visible = false;
    }

    public void FixedUpdate()
    {
        if (Gamepad.current != null)
        {
            var dir = input.RightStick;
            newDir = Vector2.Lerp(newDir, dir, smoothing * Time.deltaTime);

            spriteRenderer.enabled = newDir.magnitude > 0.25f;
            transform.position = input.transform.position + (Vector3)newDir * Radius;
        }
        else
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = -cam.transform.position.z;
            var mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

            transform.position = mouseWorldPos;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        WHandles.DrawWireDisk(input.transform.position, Radius, Color.blue);
    }
#endif
}
