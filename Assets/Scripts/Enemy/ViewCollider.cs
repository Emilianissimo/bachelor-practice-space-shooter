using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCollider : MonoBehaviour
{
    private Enemy _enemyParent;

    void Start()
    {
        _enemyParent = GetComponentInParent<Enemy>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUP"))
        {
            PowerUp powerUP = other.GetComponent<PowerUp>();
            if (powerUP != null)
            {

                _enemyParent.ShootPowerUp(powerUP);
            }
        }
    }
}
