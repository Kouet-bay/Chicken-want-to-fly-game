using UnityEngine;

public class ReturnState : EnemyState
{
    public ReturnState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        // Set the enemy's animation to return (ใช้ enemy.Agent ตัวพิมพ์ใหญ่)
        if (enemy.Agent != null)
        {
            enemy.Agent.isStopped = false;
            // ใช้ enemy.SpawnPosition ตาม Property ที่มีใน Enemy.cs
            enemy.Agent.SetDestination(enemy.SpawnPosition); 
        }
    }

    public override void Tick(float deltaTime)
    {
        if (enemy.Player != null)
        {
            // Check if the player is within vision range (ใช้ enemy.Player และ enemy.VisionRange)
            float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
            if (distance <= enemy.VisionRange)
            {
                // Transition to the chase state (ใช้ TransitionTo)
                enemy.TransitionTo(new ChaseState(enemy));
                return;
            }
        }

        // เช็คว่ากลับถึงจุดเกิดหรือยัง (ใช้ enemy.Agent)
        if (enemy.Agent != null && !enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            // Reached the initial position, transition to idle state
            enemy.TransitionTo(new IdleState(enemy));
        }
    }
}