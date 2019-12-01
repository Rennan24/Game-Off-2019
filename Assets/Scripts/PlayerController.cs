using Spine.Unity;
using UnityEngine;

public class PlayerController : MonoFSM
{
    public float MovementSpeed = 3;
    public DelayTimer DashDelay;

    [SerializeField]
    private PlayerWeapon[] PlayerWeapons;
    private int curWeaponIndex = 0;

    public PlayerWeapon CurWeapon => PlayerWeapons[curWeaponIndex];

    [Header("References:")]
    [SerializeField]
    private MovementController controllerRef;

    public HealthBehaviour Health;

    [SerializeField]
    private DashBehaviour dash;

    [SerializeField]
    private SkeletonAnimation animator;

    [SerializeField]
    private AnimationReferenceAsset deathAnim;

    [SerializeField, SpineBone]
    private string ArmBoneName;

    [SerializeField, SpineBone]
    private string bulletSpawnTargetName;

    private Spine.Bone bulletSpawnTarget;
    private Spine.Bone armBone;

    private int facing = -1;
    private float armRotation = 0f;

    public bool IsDead => Health.IsKilled;

    [SerializeField]
    private InputController input;

    private void Start()
    {
        Initialize<Idle>(new IState[] {
            new Idle(this),
            new Run(this),
            new Death(this),
        });

        armBone = animator.skeleton.FindBone(ArmBoneName);
        bulletSpawnTarget = animator.skeleton.FindBone(bulletSpawnTargetName);

        // Flip the player to the facing direction
        var mouseTargetDir = input.MouseWorldPos - transform.position;
        facing = mouseTargetDir.x > 0f ? 1 : -1;
        animator.transform.ScaleX(facing);

        animator.UpdateWorld += AnimatorOnUpdateLocal;

        Health.Killed += (pos, dir) => {
            input.OnDisable();
            ChangeState<Death>();
        };
    }

    private void AnimatorOnUpdateLocal(ISkeletonAnimation animated)
    {
        armBone.Rotation = (armRotation - 90) * facing;
    }

    private void ChangeWeapon(int newWeaponIndex)
    {
        curWeaponIndex = newWeaponIndex;

        if (curWeaponIndex < 0)
            curWeaponIndex = PlayerWeapons.Length - 1;

        if (curWeaponIndex > PlayerWeapons.Length - 1)
            curWeaponIndex = 0;

        // Clears the attacking track
        CurWeapon.SwappedTo();
        animator.state.ClearTrack(1);
        CurState.Enter();
    }

    public void AmmoPickedUp(AmmoPickup ammo)
    {
        // Replenish all the ammo for the weapons of the same type,
        // Yes this does do it for ALL weapons of the same type.
        // That's how the Game Jam Code be
        foreach (var weapon in PlayerWeapons)
        {
            if (weapon.WeaponType == ammo.Type)
                weapon.AmmoCount += ammo.Amount;
        }
    }

    protected override void Tick()
    {
        var mouseTargetDir = (input.MouseWorldPos - transform.position).normalized;

        if (facing == 1 && mouseTargetDir.x > 0.3f)
        {
            facing = -1;
            animator.transform.ScaleX(facing);
        }

        if(facing == -1 && mouseTargetDir.x < -0.3f)
        {
            facing = 1;
            animator.transform.ScaleX(facing);
        }

        armRotation = Mathf.Atan2(mouseTargetDir.y, mouseTargetDir.x) * Mathf.Rad2Deg;

        if (input.FireDown)
        {
            var bulletSpawnPos = bulletSpawnTarget.GetWorldPosition(transform);
            var fired = CurWeapon.Attack(bulletSpawnPos, mouseTargetDir.normalized, armRotation);

            if(fired)
                animator.state.SetAnimation(1, CurWeapon.AttackAnim, false);
        }

        if (input.SwapLeft)
            ChangeWeapon(curWeaponIndex - 1);

        if (input.SwapRight)
            ChangeWeapon(curWeaponIndex + 1);

        if (input.DashPressed && DashDelay.AutoReady())
        {
            dash.Dash(input.MoveDir);
        }
    }

    protected override void FixedTick()
    {
        if (!dash.IsDashing && !Health.IsKilled)
        {
            if (input.MoveDir.sqrMagnitude > 0.1f)
            {
                ChangeState(typeof(Run));
                controllerRef.Move(input.MoveDir * MovementSpeed);
            }
            else
            {
                ChangeState(typeof(Idle));
            }
        }
    }

    private class Idle : State<PlayerController>
    {
        public Idle(PlayerController agent) : base(agent) { }

        public override void Enter()
        {
            Agent.animator.state.SetAnimation(0, Agent.CurWeapon.IdleAnim, true);
        }
    }

    private class Run : State<PlayerController>
    {
        public Run(PlayerController agent) : base(agent) { }

        public override void Enter()
        {
            Agent.animator.state.SetAnimation(0, Agent.CurWeapon.RunAnim, true);
        }
    }

    private class Death: State<PlayerController>
    {
        public Death(PlayerController agent): base(agent) { }

        public override void Enter()
        {
            Agent.animator.state.ClearTrack(1);
            Agent.animator.SetAnimation(0, Agent.deathAnim, false);
        }
    }
}
