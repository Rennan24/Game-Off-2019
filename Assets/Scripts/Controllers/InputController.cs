using UnityEngine;

public class InputController : MonoBehaviour
{
    public Vector3 MouseScreenPos { get; private set; }

    public Vector3 MouseWorldPos { get; private set; }

    public Vector2 MoveDir { get; private set; }

    public bool FireDown { get; private set; }

    public bool DashPressed { get; private set; }

    private Camera cam;
    private InputMapping input;

    private void Awake()
    {
        cam = Camera.main;

        input = new InputMapping();
    }

    private void Update()
    {
        MouseScreenPos = Input.mousePosition;

        // Calculates the MouseWorldPos
        var mouseTempPos = new Vector3(MouseScreenPos.x, MouseScreenPos.y, -cam.transform.position.z);
        MouseWorldPos = cam.ScreenToWorldPoint(mouseTempPos);

        MoveDir = input.Player.Move.ReadValue<Vector2>();
        FireDown = input.Player.Fire.ReadValue<float>() > 0.1f;
        DashPressed = input.Player.Boost.triggered;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
