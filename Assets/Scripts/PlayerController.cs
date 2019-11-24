using Spine.Unity;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 3;

    public Vector3 GunOffset;

    private int curDashCount;

    [Header("References:")]
    [SerializeField]
    private TopdownController controllerRef;

    [SerializeField]
    private GunBehaviour gunRef;

    [SerializeField]
    private DashBehaviour dash;

    [SerializeField]
    private SkeletonAnimation animator;

    [SerializeField, SpineBone]
    private string ArmBoneName;

    [SerializeField, SpineBone]
    private string HipName;

    [SerializeField, SpineBone]
    private string ProjectileSpawnPointName;

    private Spine.Bone bulletSpawnTarget;
    private Spine.Bone ArmBone;
    private Spine.Bone hipBone;

    private int facing = -1;

    [SerializeField]
    private InputController input;


    private void Start()
    {
        hipBone = animator.skeleton.FindBone(HipName);
        ArmBone = animator.skeleton.FindBone(ArmBoneName);
        bulletSpawnTarget = animator.skeleton.FindBone(ProjectileSpawnPointName);

        // Flip the player to the facing direction
        var mouseTargetDir = input.MouseWorldPos - (transform.position + GunOffset);
        facing = mouseTargetDir.x > 0f ? 1 : -1;
    }

    private void Update()
    {
        var mouseTargetDir = input.MouseWorldPos - (transform.position + GunOffset);

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


        var armRotation = Mathf.Atan2(mouseTargetDir.y, mouseTargetDir.x) * Mathf.Rad2Deg - 90f;

        ArmBone.Rotation = armRotation * facing;
        animator.skeleton.UpdateWorldTransform();

        var gunPos = bulletSpawnTarget.GetWorldPosition(transform);
//        var gunRot = Quaternion.Euler(0, 0, projectileSpawnPoint.WorldRotationX);
        var gunRot = Quaternion.Euler(0, 0, armRotation + 90f);
        gunRef.transform.SetPositionAndRotation(gunPos, gunRot);
//        gunRef.transform.SetPositionAndRotation(transform.position + GunOffset, gunRot);

        if (input.FireDown)
            gunRef.Fire(mouseTargetDir.normalized, gunRot);

        if (input.DashPressed)
            dash.Dash(input.MoveDir);
    }

    private void FixedUpdate()
    {
        if (!dash.IsDashing)
        {
            controllerRef.Move(input.MoveDir * MovementSpeed);
        }
    }
}
