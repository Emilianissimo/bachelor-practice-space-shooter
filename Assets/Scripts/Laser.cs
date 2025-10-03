using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    public bool isEnemy = false;
    private bool _enemyShootBackwards = false;

    /// <summary>
    /// Rinning new laser and destroy laser and its parent if it exists after 2 seconds (which is enough with speed 5m per sec
    /// </summary>
    void Update()
    {
        Vector3 direction;
        if (_enemyShootBackwards && isEnemy)
        {
            direction = Vector3.up;
        }
        else
        {
            direction = isEnemy ? Vector3.down : Vector3.up;
        }
        transform.Translate(_speed * Time.deltaTime * direction);
        Destroy(gameObject, 2);
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject, 2);
        }
    }

    /// <summary>
    /// isEnemy value setter to determine which laser is it
    /// </summary>
    /// <param name="value">Boolean value to set</param>
    public void setIsEnemy(bool value)
    {
        isEnemy = value;
    }

    public void setEnemyShootBackwards(bool value)
    {
        _enemyShootBackwards = value;
    }

    /// <summary>
    /// Check on damage player
    /// </summary>
    /// <param name="other">Laser, Player</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isEnemy)
        {
            if (other.transform.TryGetComponent<Player>(out var player)) player.Damage(1);
            Destroy(gameObject);
        }
    }
}
