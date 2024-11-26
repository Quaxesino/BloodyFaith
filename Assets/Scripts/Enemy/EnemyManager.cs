using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] Vector2 timeRangeBetweenAttacks = new Vector2(1, 4);

    [SerializeField] CombatController player;
    [field: SerializeField] public LayerMask EnemyLayer { get; private set; }
    public static EnemyManager i {  get; private set; }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        i = this;
    }
    public List<EnemyController> enemiesInRange = new List<EnemyController>();
    float notAttackingTimer = 2;

    public void AddEnemyInRange(EnemyController enemy)
    {
        if (!enemiesInRange.Contains(enemy))
            enemiesInRange.Add(enemy);
        //Eðer menzildeyse ekle düþmaný Manager'a.

    }

    public void RemoveEnemyInRange(EnemyController enemy)
    {
        enemiesInRange.Remove(enemy);

        if (enemy == player.TargetEnemy)
        {
            enemy.MeshHighlighter?.HighlightMesh(false); 
            player.TargetEnemy = GetClosestEnemyToDirection(player.GetTargetDir());
            player.TargetEnemy?.MeshHighlighter?.HighlightMesh(false);
        }
    }

    float timer = 0f;

    private void Update()
    {
        if (enemiesInRange.Count == 0) return;

        if (!enemiesInRange.Any(e => e.IsInState(EnemyStates.Attack)))
        {
            if (notAttackingTimer > 0)
                notAttackingTimer -= Time.deltaTime;

            if (notAttackingTimer <= 0)
            {
                var attackingEnemy = SelectEnemyForAttack();

                if (attackingEnemy != null)
                {
                attackingEnemy.ChangeState(EnemyStates.Attack);
                notAttackingTimer = Random.Range(timeRangeBetweenAttacks.x, timeRangeBetweenAttacks.y);
                }
            }
            

        }
        if (timer > 0.1f)
        {
            timer = 0f;
            var closestEnemy = GetClosestEnemyToDirection(player.GetTargetDir());
            if (closestEnemy != null && closestEnemy != player.TargetEnemy)
            {
                var prevEnemy = player.TargetEnemy;
                player.TargetEnemy = closestEnemy;

                player?.TargetEnemy?.MeshHighlighter.HighlightMesh(false);
                prevEnemy?.MeshHighlighter?.HighlightMesh(false);
            }

        }

        timer += Time.deltaTime;
    }

    EnemyController SelectEnemyForAttack()
    {
       return enemiesInRange.OrderByDescending(e => e.CombatMovementTimer).FirstOrDefault(e => e.Target != null && e.IsInState(EnemyStates.CombatMovementState));
    }


    public EnemyController GetAttackingEnemy()
    {
        return enemiesInRange.FirstOrDefault(e => e.IsInState(EnemyStates.Attack));
    }
    public EnemyController GetClosestEnemyToDirection(Vector3 direction)
    {
        //Enemy-Player pozisyonu aradaki açýsal fark ve sonuç olarak aradaki farkýn en az olduðu bizim en yakýn düþmanýmýz olur.
        float minDistance = Mathf.Infinity;
        EnemyController closestEnemy = null;    

        foreach (var enemy in enemiesInRange)
        {
            var vecToEnemy = enemy.transform.position - player.transform.position;
            vecToEnemy.y = 0;
            //Distance to the targetingDir linr will be v * sin(theta)
            float angle = Vector3.Angle(direction, vecToEnemy);
            float distance = vecToEnemy.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}
