using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] EnemyController enemy;

    private void Awake()
    {
        enemy.VisionSensor = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>();
        if (fighter != null)
        {
            enemy.TargetsInRange.Add(fighter);
            EnemyManager.i.AddEnemyInRange(enemy);
        }
            
         
    }

    private void OnTriggerExit(Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>();

        if (fighter != null)
        {
            enemy.TargetsInRange.Remove(fighter);
            EnemyManager.i.RemoveEnemyInRange(enemy);
        }

    }
}
