using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogicalHitbox : MonoBehaviour
{
    private Enemy _enemyParent;

    void Start()
    {
        _enemyParent = GetComponentInParent<Enemy>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Laser laser = other.GetComponent<Laser>();
            if (laser != null && !laser.isEnemy)
            {

                _enemyParent.EvadeLaser();
            }
        }
    }
}
