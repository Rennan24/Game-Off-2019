using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 3;

    public GunBehaviour Gun;
    public Vector3 GunOffset;

    private InputMapping input;

    private int curDashCount;

    [Header("References:")]
    [SerializeField]
    private TopdownController controllerRef;

    [SerializeField]
    private SpriteRenderer playerRenderer;

    [SerializeField]
    private SpriteRenderer gunRenderer;

    [SerializeField]
    private DashBehaviour dash;

    private Camera camera;

    private void Awake()
    {
        input = new InputMapping();

        camera = Camera.main;
    }

    private void Update()
    {
        var dir = input.Player.Move.ReadValue<Vector2>();
        var boost = input.Player.Boost.triggered;
        var fired = input.Player.Fire.ReadValue<float>() > 0.1f;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -camera.transform.position.z;
        var mouseWorldPos = camera.ScreenToWorldPoint(mouseScreenPos);

        var mouseTargetDir = mouseWorldPos - (transform.position + GunOffset);

        var gunRotZ = Mathf.Atan2(mouseTargetDir.y, mouseTargetDir.x) * Mathf.Rad2Deg;
        var gunPos = transform.position + GunOffset + mouseTargetDir.normalized;
        var gunQuat = Quaternion.Euler(0, 0, gunRotZ);

        Gun.transform.SetPositionAndRotation(gunPos, gunQuat);
//        Gun.transform.rotation = gunQuat;

        // flip renderer if mouse is to the left of the player
        playerRenderer.flipX = mouseTargetDir.x < 0;
        gunRenderer.flipY = mouseTargetDir.x < 0;

        if (fired)
         Gun.Fire(mouseTargetDir.normalized);

        if (boost)
            dash.Dash(dir);

        if(!dash.IsDashing)
            controllerRef.Move(dir * MovementSpeed);
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
