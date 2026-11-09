using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    public enum LostSightBehavior { StandStill, Patrol }

    [Header("Enemy Identity")]
    [SerializeField] private string enemyType;

    [Header("AI Settings")]
    [SerializeField] private float visionRange = 6f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Lost Sight Behavior")]
    [SerializeField] private LostSightBehavior lostSightBehavior = LostSightBehavior.StandStill;
    [SerializeField] private Transform[] patrolPoints;

    // Components & References
    private NavMeshAgent agent;
    private Animator animator;
    private Player player;
    private Vector3 spawnPosition;
    private EnemyState currentState;

    // Public Getters
    public NavMeshAgent Agent => agent;
    public Player Player => player;
    public Vector3 SpawnPosition => spawnPosition;
    public float VisionRange => visionRange;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public Transform[] PatrolPoints => patrolPoints;

    public string EnemyType
    {
        get => enemyType;
        private set => enemyType = value;
    }

    // ==========================================
    // UNITY LIFECYCLE METHODS
    // ==========================================
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        spawnPosition = transform.position;
    }

    protected override void Start()
    {
        base.Start();
        player = FindAnyObjectByType<Player>();
        TransitionTo(CreateInitialState());
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Tick(Time.deltaTime);
        }
    }

    // ==========================================
    // STATE MACHINE METHODS
    // ==========================================
    public void TransitionTo(EnemyState newState)
    {
        currentState = newState;
        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    private EnemyState CreateInitialState()
    {
        return lostSightBehavior == LostSightBehavior.Patrol ? new PatrolState(this) : new IdleState(this);
    }

    public EnemyState CreateLostSightState()
    {
        return lostSightBehavior == LostSightBehavior.Patrol ? new PatrolState(this) : new ReturnState(this);
    }

    public void SetMovementState(int state)
    {
        if (animator != null)
        {
            animator.SetInteger("State", state);
        }
    }

    // ==========================================
    // COMBAT & DAMAGE METHODS
    // ==========================================
    public virtual void PerformAttack()
    {
        if (player != null && player.Hp <= 0)
        {
            if (currentState != CreateLostSightState())
            {
                TransitionTo(CreateLostSightState());
            }
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Attack(player);
    }

    public override void Attack(IDamageable target)
    {
        if (target != null)
        {
            int damage = attackPoints;
            target.TakeDamage(damage, this);
            Debug.Log($"{EnemyType} attacks {(target as Component)?.name} for {damage} damage!");
        }
    }

    public override void TakeDamage(int damage, Character attacker)
    {
        int effectiveDamage = Mathf.Max(damage - defensePoints, 0);
        Hp -= effectiveDamage;
        Debug.Log($"{EnemyType} takes {effectiveDamage} damage! Remaining HP: {Hp}");

        if (Hp <= 0)
        {
            Die(attacker);
        }
    }

    public override void Die(Character attacker)
    {
        Debug.Log($"{EnemyType} has been defeated!");
        SetMovementState(-1);

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        this.enabled = false; 
        
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        Destroy(gameObject, 3f);
    }

    // ==========================================
    // GIZMOS (DEBUG VISUALIZATION)
    // ==========================================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector3 origin = Application.isPlaying ? spawnPosition : transform.position;

        if (lostSightBehavior == LostSightBehavior.StandStill)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(origin, 0.3f);
        }
        else if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] == null) continue;

                Gizmos.DrawWireSphere(patrolPoints[i].position, 0.3f);

                Transform next = patrolPoints[(i + 1) % patrolPoints.Length];
                if (next != null)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, next.position);
                }
            }
        }
    }
}