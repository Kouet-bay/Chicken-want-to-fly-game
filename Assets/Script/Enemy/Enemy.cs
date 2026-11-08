using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Character
{
    // เลือกได้ทีละตัวว่าหลุดระยะมองเห็นแล้วจะกลับไปยืนเฉยๆ ที่จุดเกิด หรือเดิน patrol ต่อ
    public enum LostSightBehavior { StandStill, Patrol }

    [SerializeField] private string enemyType;
    [SerializeField] private int experienceReward = 100;

    [Header("Loot — สุ่มจำนวน Gold ที่ดรอปตอนตายในช่วง min-max")]
    [SerializeField] private int minGoldDrop = 5;
    [SerializeField] private int maxGoldDrop = 15;

    [Header("AI Settings — ปรับต่างกันได้ทีละ Enemy/ทีละ prefab")]
    [SerializeField] private float visionRange = 6f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Lost Sight Behavior")]
    [SerializeField] private LostSightBehavior lostSightBehavior = LostSightBehavior.StandStill;
    [SerializeField] private Transform[] patrolPoints; // ใช้เฉพาะตอนเลือก Patrol

    private NavMeshAgent agent;
    private Animator animator;
    private Player player;
    private Vector3 spawnPosition;
    private EnemyState currentState;

    public NavMeshAgent Agent => agent;
    public Player Player => player;
    public Vector3 SpawnPosition => spawnPosition;
    public float VisionRange => visionRange;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public Transform[] PatrolPoints => patrolPoints;

    public string EnemyType
    {
        get { return enemyType; }
        private set { enemyType = value; }
    }

    public int ExperienceReward
    {
        get { return experienceReward; }
        private set { experienceReward = value; }
    }

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
        currentState.Tick(Time.deltaTime);
    }

    public void TransitionTo(EnemyState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    // ตอนเริ่มเกม: StandStill เริ่มยืนเฉยที่จุดเกิด, Patrol เริ่มเดินตามจุด patrol เลย
    private EnemyState CreateInitialState()
    {
        return lostSightBehavior == LostSightBehavior.Patrol ? new PatrolState(this) : new IdleState(this);
    }

    // ตอนหลุดระยะมองเห็นระหว่างไล่ล่า: StandStill เดินกลับจุดเกิดก่อนค่อยยืนเฉย, Patrol กลับไปเดิน patrol ต่อทันที
    public EnemyState CreateLostSightState()
    {
        return lostSightBehavior == LostSightBehavior.Patrol ? new PatrolState(this) : new ReturnState(this);
    }

    // ให้แต่ละ state สั่งอนิเมชันตามที่ทำอยู่ (0 = idle, 1 = walk) ไม่ผูกกับ state ใด state หนึ่งโดยเฉพาะ
    public void SetMovementState(int state)
    {
        if (animator != null)
        {
            animator.SetInteger("State", state);
        }
    }

    // จุดต่อยอดสำหรับอนาคต: Enemy พันธุ์โจมตีระยะไกลแค่ override เมธอดนี้ตัวเดียว
    // (เช่น ยิงกระสุนแทนการโจมตีประชิด) โดยไม่ต้องแตะ AttackState เลย
    public virtual void PerformAttack()
    {
        if (player.Hp <= 0)
        {
            if(currentState != CreateLostSightState())
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
        if (attacker != null)
        {
            GameManager.Instance?.GainExperience(ExperienceReward);

            int goldDropped = Random.Range(minGoldDrop, maxGoldDrop + 1); // Random.Range(int,int) ไม่รวมค่า max จึง +1
            GameManager.Instance?.AddGold(goldDropped);
            Debug.Log($"{EnemyType} dropped {goldDropped} gold!");
        }
        Debug.Log($"{EnemyType} has been defeated!");
        SetMovementState(-1);
        agent.isStopped = true;
        Collider[] colliders = GetComponentsInChildren<Collider>();
        this.enabled = false; // Disable the enemy's script to prevent further actions
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        Destroy(gameObject, 3); // Remove the enemy from the scene after 3 seconds
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ก่อนกด Play ยังไม่มี spawnPosition (ตั้งค่าจริงใน Awake) ใช้ตำแหน่งปัจจุบันแทนไปก่อน
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