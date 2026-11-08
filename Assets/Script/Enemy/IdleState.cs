using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        // เปลี่ยนจาก enemy.agent เป็น enemy.Agent
        if (enemy.Agent != null)
        {
            enemy.Agent.isStopped = true;
        }
        // You can also set an idle animation here if you have one
    }

    public override void Tick(float deltaTime)
    {
        if (enemy.Player == null)
        {
            return; // No player to chase
        }

        // Check if the player is within vision range
        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distance <= enemy.VisionRange)
        {
            // Transition to the chase state using TransitionTo
            enemy.TransitionTo(new ChaseState(enemy));
        }
    }
}