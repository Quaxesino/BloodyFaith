using UnityEngine;

public class RetreatAfterAttackState : State<EnemyController>
{
    [SerializeField] float backwardWalkSpeed = 1.5f;
    [SerializeField] float distanceToRetreat = 3f;
    EnemyController enemy;
    Vector3 targetPos;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        targetPos = enemy.Target.transform.position;
    }

    public override void Execute()
    {
        if (Vector3.Distance(enemy.transform.position,targetPos) >= distanceToRetreat)
        {
            enemy.ChangeState(EnemyStates.CombatMovementState);
            return;
        }

        var vecToTarget = enemy.Target.transform.position - enemy.transform.position;
        enemy.NavAgent.Move(-vecToTarget.normalized * backwardWalkSpeed * Time.deltaTime);

        vecToTarget.y = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(vecToTarget), 500 * Time.deltaTime);
    }
}
