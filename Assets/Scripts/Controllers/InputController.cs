using UnityEngine;
using UnityEngine.InputSystem;

public class InputController: MonoBehaviour
{
    public Vector3 MouseScreenPos { get; private set; }

    public Vector3 MouseWorldPos { get; private set; }

    public Vector2 MoveDir { get; private set; }

    public bool FireDown { get; private set; }

    public bool DashPressed { get; private set; }

    public bool SwapLeft { get; private set; }
    public bool SwapRight { get; private set; }

    public Vector2 RightStick { get; private set; }

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
//        MouseScreenPos = input.Player.MousePos.ReadValue<Vector2>();

        // Calculates the MouseWorldPos
        var mouseTempPos = new Vector3(MouseScreenPos.x, MouseScreenPos.y, -cam.transform.position.z);
        MouseWorldPos = cam.ScreenToWorldPoint(mouseTempPos);

        RightStick = input.Player.Aim.ReadValue<Vector2>();
        MoveDir = input.Player.Move.ReadValue<Vector2>();
        FireDown = input.Player.Fire.ReadValue<float>() > 0.1f;
        DashPressed = input.Player.Boost.triggered;

        var swapTriggered = input.Player.SwapWeapon.triggered;
        var swapDir = input.Player.SwapWeapon.ReadValue<float>();
        SwapRight = swapTriggered && swapDir > 0.1f;
        SwapLeft  = swapTriggered && swapDir < -0.1f;
    }

    public void OnEnable()
    {
        input.Enable();
    }

    public void OnDisable()
    {
        input.Disable();
    }
}
