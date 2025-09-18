using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 5, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    /// <summary>
    /// Calculating movement using field _speed. Loop on Y axis. Going down.
    /// </summary>
    private void CalculateMovement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        float currentPositionY = transform.position.y;
        if (currentPositionY < -5)
        {
            transform.position = new Vector3(Random.Range(-11.5f, 11.5f), 5, 0);
        }
    }

    /// <summary>
    /// On collide with laser - destroy. On player - destroy & damage.
    /// </summary>
    /// <param name="other">Laser, Player</param>
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent<Player>(out var player)) player.Damage(1);
            Destroy(this.gameObject);
        }
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null) _player.AddScore(10);
            Destroy(this.gameObject);
        }
    }
}
