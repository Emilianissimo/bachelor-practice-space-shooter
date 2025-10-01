using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHiitbox : MonoBehaviour
{
    private Enemy _enemyParent;

    void Start()
    {
        _enemyParent = GetComponentInParent<Enemy>();
    }
    
    /// <summary>
    /// On collide with laser - destroy. On player - destroy & damage.
    /// </summary>
    /// <param name="other">Laser, Player</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        _enemyParent.TriggerController(other);
    }
}
