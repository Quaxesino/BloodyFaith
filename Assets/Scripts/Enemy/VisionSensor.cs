using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] EnemyController enemy;

    private void Awake()
    {
        if (enemy == null)
        {
            Debug.LogError("Enemy reference is not assigned in VisionSensor.");
            return;
        }

        enemy.VisionSensor = this;
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>();
        if (fighter != null)
        {
            enemy.TargetsInRange.Add(fighter);
            EnemyManager.i.AddEnemyInRange(enemy);
        }
            
         
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>();

        if (fighter != null)
        {
            enemy.TargetsInRange.Remove(fighter);
            EnemyManager.i.RemoveEnemyInRange(enemy);
        }

    }
}
