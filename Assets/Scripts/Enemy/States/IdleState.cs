using UnityEngine;

public class IdleState : State<EnemyController>
{
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;

        enemy.Animator.SetBool("combatMode", false);
    }

    public override void Execute()
    {
        enemy.Target = enemy.FindTarget();

        if (enemy.Target != null)
        {
            enemy.AlertNearbyEnemies();
            enemy.ChangeState(EnemyStates.CombatMovementState);

        }

    }

    public override void Exit()
    {

    }
}
