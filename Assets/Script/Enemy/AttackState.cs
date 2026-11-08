using UnityEngine;

public class AttackState : EnemyState
{
    private float attackCooldownTimer = 0f; // เวลาสะสมสำหรับคูลดาวน์การโจมตี

    public AttackState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        // Set the enemy's animation to attack (ใช้ enemy.Agent ตัวพิมพ์ใหญ่)
        if (enemy.Agent != null)
        {
            enemy.Agent.isStopped = true;
        }

        attackCooldownTimer = 0f; // รีเซ็ตคูลดาวน์เมื่อเข้าสู่สเตท

        // โจมตีทันทีถ้ามีฟังก์ชัน Attack ใน Enemy (ใช้ enemy.Player ตัวพิมพ์ใหญ่)
        if (enemy.Player != null)
        {
            enemy.Attack(enemy.Player); 
        }
    }

    public override void Tick(float deltaTime)
    {
        if (enemy.Player == null)
        {
            return; // No player to attack
        }

        // หันหน้าเข้าหา Player เสมอตอนอยู่ในระยะโจมตี
        Vector3 directionToPlayer = (enemy.Player.transform.position - enemy.transform.position).normalized;
        directionToPlayer.y = 0f;
        if (directionToPlayer != Vector3.zero)
        {
            enemy.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }

        // เช็คระยะห่าง (ใช้ enemy.AttackRange ตัวพิมพ์ใหญ่)
        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distance > enemy.AttackRange)
        {
            // Player is out of attack range, transition to chase state (ใช้ TransitionTo)
            enemy.TransitionTo(new ChaseState(enemy));
            return;
        }

        attackCooldownTimer += deltaTime;
        
        // เช็คคูลดาวน์ (ใช้ enemy.AttackCooldown ตัวพิมพ์ใหญ่ หากมี หรือกำหนดค่าตายตัว)
        if (attackCooldownTimer >= enemy.AttackCooldown)
        {
            enemy.Attack(enemy.Player);
            attackCooldownTimer = 0f; // Reset the cooldown after attacking
        }
    }
}