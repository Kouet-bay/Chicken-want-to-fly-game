using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        // Set the enemy's animation to chase
        if (enemy.Agent != null)
        {
            enemy.Agent.isStopped = false;
        }
        // You can also set a chase animation here if you have one
    }

    public override void Tick(float deltaTime)
    {
        if (enemy.Player == null)
        {
            return; // No player to chase
        }

        // Check if the player is within attack range
        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distance <= enemy.AttackRange)
        {
            // Transition to the attack state
            enemy.TransitionTo(new AttackState(enemy));
            return;
        }

        if (distance > enemy.VisionRange)
        {
            // Player is out of vision range, transition to idle/return or patrol based on Enemy's configuration
            enemy.TransitionTo(enemy.CreateLostSightState());
            return;
        }
        
        // Move towards the player
        if (enemy.Agent != null && enemy.Agent.enabled)
        {
            enemy.Agent.SetDestination(enemy.Player.transform.position);
        }
    }
}