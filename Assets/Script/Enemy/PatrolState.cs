using UnityEngine;

public class PatrolState : EnemyState
{
    private int currentWaypointIndex = 0;
    
    // ตัวแปรสำหรับจัดการการยืนรอที่จุด
    private float waitTimer = 0f;
    private float waitDuration = 2.0f; // เวลากี่วินาทีที่จะยืนรอที่จุด (ปรับเพิ่มลดได้ตามต้องการ)
    private bool isWaiting = false;

    public PatrolState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        if (enemy.PatrolPoints == null || enemy.PatrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points set for the enemy.");
            return;
        }

        if (enemy.Agent != null && enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.isStopped = false;
        }

        isWaiting = false;
        currentWaypointIndex = FindClosestPointIndex(); 
        
        MoveToCurrentWaypoint();
    }

    public override void Tick(float deltaTime)
    {
        // 1. เช็คผู้เล่นในระยะมองเห็น
        if (enemy.Player != null)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
            if (distance <= enemy.VisionRange)
            {
                enemy.TransitionTo(new ChaseState(enemy));
                return;
            }
        }

        // 2. ถ้ากำลังอยู่ในช่วงยืนรอที่จุด
        if (isWaiting)
        {
            waitTimer -= deltaTime;
            
            // สามารถใส่ลูกเล่นหมุนตัวหันซ้ายหันขวาเบาๆ ตรงนี้ได้ถ้าต้องการ

            // เมื่อหมดเวลาคอย ให้ไปยังจุดถัดไป
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                currentWaypointIndex = (currentWaypointIndex + 1) % enemy.PatrolPoints.Length;
                MoveToCurrentWaypoint();
            }
            return;
        }

        // 3. เช็คว่าเดินมาถึงจุดหมายหรือยัง
        if (enemy.Agent != null && enemy.Agent.isOnNavMesh && !enemy.Agent.pathPending)
        {
            if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
            {
                if (!enemy.Agent.hasPath || enemy.Agent.velocity.sqrMagnitude == 0f)
                {
                    // ถึงจุดหมายแล้ว! เริ่มต้นกระบวนการยืนรอ
                    StartWaitingAtPoint();
                }
            }
        }
    }

    private void MoveToCurrentWaypoint()
    {
        enemy.SetMovementState(1); // สมมติ 1 คืออนิเมชันเดิน (Walk)
        if (enemy.Agent != null && enemy.Agent.isOnNavMesh && enemy.PatrolPoints.Length > 0)
        {
            enemy.Agent.isStopped = false;
            enemy.Agent.SetDestination(enemy.PatrolPoints[currentWaypointIndex].position);
        }
    }

    private void StartWaitingAtPoint()
    {
        isWaiting = true;
        waitTimer = waitDuration;
        
        enemy.SetMovementState(0); // สมมติ 0 คืออนิเมชันยืนเฉยๆ (Idle)
        if (enemy.Agent != null && enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.isStopped = true; // สั่งหยุดเดินชั่วคราว
        }
    }

    private int FindClosestPointIndex()
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < enemy.PatrolPoints.Length; i++)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.PatrolPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }
}