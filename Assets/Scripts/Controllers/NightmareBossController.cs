using Spine.Unity;
using UnityEngine;

public class NightmareBossController: MonoFSM
{
    public int Amount = 18;
    public float ShootTime = 0.5f;

    public float AngleStep = 15f;

    [SerializeField]
    private MovementController movement;

    [SerializeField]
    private SkeletonAnimation animator;

    [SerializeField]
    private ProjectileController projectilesLeft;
    [SerializeField]
    private ProjectileController projectilesRight;

    private float CurAngle = 0;


    private void Start()
    {
        Initialize<Idle>(new IState[] {
            new Idle(this),
            new Shoot(this),
        });
    }


    public class Idle: State<NightmareBossController>
    {
        public Idle(NightmareBossController agent): base(agent) { }

        public override void Update()
        {
            if (TimeElapsed > Agent.ShootTime)
                Agent.ChangeState<Shoot>();
        }
    }

    public class Shoot: State<NightmareBossController>
    {
        public Shoot(NightmareBossController agent): base(agent) { }

        public override void Enter()
        {
            Agent.projectilesLeft.SpawnSequence(Agent.Amount, -Agent.CurAngle);
            Agent.projectilesRight.SpawnSequence(Agent.Amount, Agent.CurAngle);

            Agent.CurAngle += Agent.AngleStep;

            Agent.ChangeState<Idle>();
        }
    }
}
