using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 8f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private Player _player;
    private SpawnManager _spawnManager;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.LogWarning("Player not found");
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null) Debug.LogWarning("SpawnManager not found");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }
    
    /// <summary>
    /// On collide with laser - destroy. On player - destroy & damage.
    /// </summary>
    /// <param name="other">Laser, Player</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isDestroyed = false;
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent<Player>(out var player)) player.Damage(2);
            isDestroyed = true;
        }
        if (other.CompareTag("Laser") || other.CompareTag("BigBoy"))
        {
            Destroy(other.gameObject);
            if (_player != null) _player.AddScore(20);
            isDestroyed = true;
        }
        if (isDestroyed)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, .25f);
        }
    }
}
