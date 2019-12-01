using Spine.Unity;
using UnityEngine;

public class ZombieController : MonoFSM, IWaveEnemy
{
    [HideInInspector]
    public Transform Target;
    public float WanderDist;
    public float WanderSpeed;
    public float ChaseDist;
    public float ChaseSpeed;
    public float IdleTime = 1.25f;

    [SerializeField] private AnimationReferenceAsset spawnAnim;
    [SerializeField] private AnimationReferenceAsset idleAnim;
    [SerializeField] private AnimationReferenceAsset walkAnim;
    [SerializeField] private AnimationReferenceAsset deathAnim;

    private float targetDistSqr;

    [Header("References:")]
    [SerializeField]
    private MovementController movement;

    [SerializeField]
    private SkeletonAnimation animator;

    [SerializeField]
    private HealthBehaviour health;

    public WaveController WaveController { get; set; }

    private void Start()
    {
        health.Killed += OnKilled;
        Target = GameManager.Player.transform;

        Initialize<Chase>(new IState [] {
            new Chase(this),
            new Death(this),
            new Idle(this),
            new Wander(this),
        });

        var track = animator.state.SetAnimation(0, spawnAnim, false);
        Pause(track.AnimationEnd);

        CurState.Enter();
    }

    private void OnKilled(Vector3 hitpos, Vector2 hitdir)
    {
        ChangeState<Death>();
        Pause(100);
//        WaveController.EnemiesLeft--;
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

     private class Wander : State<ZombieController>
     {
         private Vector2 wanderPoint;

         public Wander(ZombieController agent) : base(agent) { }

         public override void Enter()
         {
             wanderPoint = (Vector2)T.position + UnityEngine.Random.insideUnitCircle * Agent.WanderDist;
             Agent.animator.state.SetAnimation(0, Agent.walkAnim, true);
         }

         public override void Update()
         {
             var delta = wanderPoint - (Vector2)T.position;
             var wanderDist = delta.sqrMagnitude;

             Debug.DrawLine(T.position, wanderPoint);

             T.ScaleX(delta.x > 0 ? 1 : -1);

             // Arrived at point
             if (wanderDist < 0.1f)
                 Agent.ChangeState(typeof(Idle));

             var wanderDir = delta.normalized;
             Agent.movement.Move(wanderDir * Agent.WanderSpeed);
         }
     }

     private class Idle : State<ZombieController>
     {
         public Idle(ZombieController agent) : base(agent) { }

         public override void Enter()
         {
             Agent.animator.SetAnimation(0, Agent.idleAnim, true);
         }

         public override void Update()
         {
             if (TimeElapsed > Agent.IdleTime)
                 Agent.ChangeState(typeof(Wander));
         }
     }

     private class Chase : State<ZombieController>
     {
         public Chase(ZombieController agent) : base(agent) { }

         public override void Enter()
         {
             Agent.animator.SetAnimation(0, Agent.walkAnim, true);
         }

         public override void Update()
         {
             var dir = Agent.movement.MoveTowards(Agent.Target.position, Agent.ChaseSpeed);
             T.ScaleX(dir.x > 0 ? 1 : -1);

             if(GameManager.PlayerDead)
                 Agent.ChangeState<Wander>();

//             Vector2 delta = Agent.Target.position - T.position;
//             var distSqr = delta.sqrMagnitude;
//
//             Debug.DrawLine(T.position, Agent.Target.position);
//
//             T.ScaleX(delta.x > 0 ? 1 : -1);
//
//             // Outside of the range of the target
//             if (distSqr < Agent.ChaseDist * Agent.ChaseDist)
//                 Agent.ChangeState(typeof(Idle));
//
//             var moveDir = delta.normalized;
//             Agent.movement.Move(moveDir * Agent.ChaseSpeed);
         }
     }

     private class Death : State<ZombieController>
     {
         public Death(ZombieController agent) : base(agent) { }

         public override void Enter()
         {
             Agent.animator.SetAnimation(0, Agent.deathAnim, false);
         }
     }

}
