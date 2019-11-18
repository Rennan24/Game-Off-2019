using Spine.Unity;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 3;

    public Vector3 GunOffset;

    private InputMapping input;

    private int curDashCount;

    [Header("References:")]
    [SerializeField]
    private TopdownController controllerRef;

    [SerializeField]
    private GunBehaviour gunRef;

    [SerializeField]
    private SpriteRenderer playerRenderer;

    [SerializeField]
    private SpriteRenderer gunRenderer;

    [SerializeField]
    private DashBehaviour dash;

    [SerializeField, SpineBone]
    private string AimTargetName;

    [SerializeField, SpineBone]
    private string HipName;

    [SerializeField, SpineBone]
    private string ProjectileSpawnPointName;


    private Spine.Bone projectileSpawnPoint;
    private Spine.Bone aimTarget;
    private Spine.Bone hipBone;

    private SkeletonAnimation animator;

    private int facing = -1;
    private Camera camera;

    private void Awake()
    {
        animator = GetComponent<SkeletonAnimation>();
        input = new InputMapping();

        camera = Camera.main;
    }

    private void Start()
    {
        hipBone = animator.skeleton.FindBone(HipName);
        aimTarget = animator.skeleton.FindBone(AimTargetName);
        projectileSpawnPoint = animator.skeleton.FindBone(ProjectileSpawnPointName);

        // Flip the player to the facing direction
        var mouseWorldPos = GetMouseWorldPos();
        var mouseTargetDir = mouseWorldPos - transform.position + GunOffset;
        facing = mouseTargetDir.x > 0f ? 1 : -1;
    }

    private void Update()
    {
        var dir = input.Player.Move.ReadValue<Vector2>();
        var fired = input.Player.Fire.ReadValue<float>() > 0.1f;
        var boost = input.Player.Boost.triggered;

        var mouseWorldPos = GetMouseWorldPos();
        var mouseTargetDir = mouseWorldPos - transform.position + GunOffset;

        if (facing == 1 && mouseTargetDir.x > 0.3f)
        {
            facing = -1;
            hipBone.ScaleX = facing;
        }

        if(facing == -1 && mouseTargetDir.x < -0.3f)
        {
            facing = 1;
            hipBone.ScaleX = facing;
        }

        var gunPos = projectileSpawnPoint.GetWorldPosition(transform);
        var gunRot = Quaternion.Euler(0, 0, projectileSpawnPoint.WorldRotationX);
        gunRef.transform.SetPositionAndRotation(gunPos, gunRot);

        aimTarget.SetLocalPosition(mouseWorldPos - transform.position);

        if (fired)
            gunRef.Fire(mouseTargetDir.normalized);

        if (boost)
            dash.Dash(dir);

        if(!dash.IsDashing)
            controllerRef.Move(dir * MovementSpeed);
    }

    private Vector3 GetMouseWorldPos()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -camera.transform.position.z;
        return camera.ScreenToWorldPoint(mouseScreenPos);
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
