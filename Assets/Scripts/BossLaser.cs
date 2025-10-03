using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    
    void Update()
    {
        Vector3 direction;
        direction = Vector3.down;
        transform.Translate(_speed * Time.deltaTime * direction);
        Destroy(gameObject, 2);
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject, 2);
        }
    }

    /// <summary>
    /// Check on damage player
    /// </summary>
    /// <param name="other">Laser, Player</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent<Player>(out var player)) player.Damage(1);
            Destroy(gameObject);
        }
    }
}
