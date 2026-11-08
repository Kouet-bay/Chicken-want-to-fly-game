using UnityEngine;

public class PatrolState : EnemyState
{
    private int currentWaypointIndex = 0;

    public PatrolState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        // ใช้ enemy.PatrolPoints (ตัว P พิมพ์ใหญ่)
        if (enemy.PatrolPoints == null || enemy.PatrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points set for the enemy.");
            return;
        }

        // Set the enemy's animation to patrol (ใช้ enemy.Agent)
        if (enemy.Agent != null)
        {
            enemy.Agent.isStopped = false;
        }

        currentWaypointIndex = FindClosestPointIndex(); // Start from the closest patrol point
        
        if (enemy.Agent != null && enemy.PatrolPoints.Length > 0)
        {
            enemy.Agent.SetDestination(enemy.PatrolPoints[currentWaypointIndex].position);
        }
    }

    public override void Tick(float deltaTime)
    {
        // ใช้ enemy.Player (ตัว P พิมพ์ใหญ่)
        if (enemy.Player != null)
        {
            // Check if the player is within vision range (ใช้ enemy.VisionRange)
            float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
            if (distance <= enemy.VisionRange)
            {
                // Transition to the chase state (ใช้ TransitionTo)
                enemy.TransitionTo(new ChaseState(enemy));
                return;
            }
        }

        // ใช้ enemy.Agent
        if (enemy.Agent != null && !enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            // Move to the next patrol point (ใช้ enemy.PatrolPoints)
            currentWaypointIndex = (currentWaypointIndex + 1) % enemy.PatrolPoints.Length;
            enemy.Agent.SetDestination(enemy.PatrolPoints[currentWaypointIndex].position);
        }
    }

    private int FindClosestPointIndex()
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        // ใช้ enemy.PatrolPoints
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