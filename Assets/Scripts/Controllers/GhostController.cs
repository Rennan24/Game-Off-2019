using System;
using UnityEngine;

public class GhostController: MonoFSM
{
    public float MaxFlySpeed;
    public float FlySmoothing;

    [Header("References:")]
    [SerializeField]
    private MovementController movement;

    private Transform player;

    private HealthBehaviour health;

    private void Start()
    {
        player = GameManager.Player.transform;

        health = GetComponent<HealthBehaviour>();

        health.Killed += (pos, dir) => { Pause(100); };

        Initialize<Chase>(new IState[] {
            new Chase(this),
        });
    }

#if UNITY_EDITOR
    private void Reset()
    {
        movement = GetComponent<MovementController>();
    }
#endif

    public class Chase: State<GhostController>
    {
        private float speed;

        public Chase(GhostController agent): base(agent) { }

        public override void Update()
        {
            speed = Mathf.SmoothStep(speed, Agent.MaxFlySpeed, Agent.FlySmoothing * Time.deltaTime);
        }

        public override void FixedUpdate()
        {
            var moveAway = GameManager.PlayerDead ? -1 : 1;
            var dir = Agent.movement.MoveTowards(Agent.player.position, speed * -moveAway);
            T.ScaleX(dir.x > 0 ? 1 : -1);
        }
    }

    public class Death: State<GhostController>
    {
        public Death(GhostController agent): base(agent) { }
    }
}
