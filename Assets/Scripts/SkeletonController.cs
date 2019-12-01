using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class SkeletonController: MonoFSM
{
    [HideInInspector]
    public Transform Target;

    public ConstantVelocity Scythe;
    public float ScytheSpeed;
    public Vector2 ScytheOffset;
    public float WanderDist;
    public float WanderSpeed;
    public float ChaseDist;
    public float ChaseSpeed;
    public float ChaseTime = 1.25f;

    [SerializeField] private AnimationReferenceAsset spawnAnim;
    [SerializeField] private AnimationReferenceAsset idleAnim;
    [SerializeField] private AnimationReferenceAsset walkAnim;
    [SerializeField] private AnimationReferenceAsset attackAnim;
    [SerializeField] private AnimationReferenceAsset deathAnim;

    private float targetDistSqr;

    [Header("References:")]
    [SerializeField]
    private MovementController movement;

    [SerializeField]
    private SkeletonAnimation animator;

    [SerializeField]
    private HealthBehaviour health;

    private void Start()
    {
        Target = GameManager.Player.transform;

        Initialize<Chase>(new IState[] {
            new Chase(this),
            new Attack(this),
            new Death(this),
            new Idle(this),
        });

//        var track = animator.state.SetAnimation(0, spawnAnim, false);
//        Pause(track.AnimationEnd);
    }

//    protected override void FixedTick()
//    {
//        var targetDir = Target.position - transform.position;
//        targetDistSqr = targetDir.sqrMagnitude;
//
//        if (targetDistSqr < ChaseDist * ChaseDist)
//            ChangeState(typeof(Chase));
//    }

#if UNITY_EDITOR
    private void Reset()
    {
        animator = GetComponentInChildren<SkeletonAnimation>();
        movement = GetComponentInChildren<MovementController>();
        health = GetComponentInChildren<HealthBehaviour>();
    }

    private void OnDrawGizmosSelected()
    {
        WHandles.DrawWireDisk(transform.position, WanderDist, new Color(1, 1, 0, 0.7f));
        WHandles.DrawWireDisk(transform.position, ChaseDist, new Color(0, 1, 0, 0.7f));
    }
#endif

//     private class Wander : State<SkeletonController>
//     {
//         private Vector2 wanderPoint;
//
//         public Wander(SkeletonController agent) : base(agent) { }
//
//         public override void Enter()
//         {
//             wanderPoint = (Vector2)T.position + UnityEngine.Random.insideUnitCircle * Agent.WanderDist;
//             Agent.animator.state.SetAnimation(0, Agent.walkAnim, true);
//         }
//
//         public override void Update()
//         {
//             var delta = wanderPoint - (Vector2)T.position;
//             var wanderDist = delta.sqrMagnitude;
//
//             Debug.DrawLine(T.position, wanderPoint);
//
//             // Arrived at point
//             if (wanderDist < 0.1f)
//                 Agent.ChangeState(typeof(Idle));
//
//             var wanderDir = delta.normalized;
//             Agent.movement.Move(wanderDir * Agent.WanderSpeed);
//         }
//     }

    private class Idle: State<SkeletonController>
    {
        public Idle(SkeletonController agent): base(agent) { }

        public override void Enter()
        {
            Agent.animator.SetAnimation(0, Agent.idleAnim, true);
        }
    }

    private class Attack: State<SkeletonController>
    {
        private float attackTime;

        public Attack(SkeletonController agent): base(agent)
        {
            agent.animator.state.Event += OnAttack;
        }

        private void OnAttack(TrackEntry entry, Spine.Event e)
        {
            var delta = T.GetDelta(Agent.Target);
            var dir = delta.normalized;

            Vector3 offset = Agent.ScytheOffset;
            offset.x *= Mathf.Sign(dir.x);

            var velocity = Instantiate(Agent.Scythe, T.position + offset, Quaternion.identity);
            velocity.Value = dir * Agent.ScytheSpeed;
        }

        public override void Enter()
        {
            var track = Agent.animator.state.SetAnimation(0, Agent.attackAnim, false);
//            Agent.ChangeState(typeof(Idle), track.AnimationEnd);

            var deltaX = T.GetDelta(Agent.Target).x;
            T.ScaleX(deltaX > 0 ? 1 : -1);

            attackTime = track.AnimationEnd;
        }

        public override void Update()
        {
//            Debug.Log($"{TimeElapsed} {attackTime}");
            if (TimeElapsed > attackTime)
                Agent.ChangeState<Chase>();
        }
    }

    private class Chase: State<SkeletonController>
    {
        public Chase(SkeletonController agent): base(agent) { }

        public override void Enter()
        {
            Agent.animator.state.SetAnimation(0, Agent.walkAnim, true);
        }

        public override void Update()
        {
            Vector2 delta = T.GetDelta(Agent.Target);
            var distSqr = delta.sqrMagnitude;

            Debug.DrawLine(T.position, Agent.Target.position);

            // Inside of the range of the attack target
            if (GameManager.PlayerDead)
                Agent.ChangeState<Idle>();

            if (TimeElapsed > Agent.ChaseTime && distSqr < Agent.ChaseDist * Agent.ChaseDist)
                Agent.ChangeState(typeof(Attack));

            var moveDir = delta.normalized;
            Agent.movement.Move(moveDir * Agent.ChaseSpeed);
            T.ScaleX(moveDir.x > 0 ? 1 : -1);
        }
    }

    private class Death: State<SkeletonController>
    {
        public Death(SkeletonController agent): base(agent) { }

        public override void Enter()
        {
            Agent.animator.SetAnimation(0, Agent.deathAnim, false);
        }
    }
}
