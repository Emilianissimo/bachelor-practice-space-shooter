using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;

    private AudioSource _audioSource;

    private Player _player;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 5, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.LogWarning("Player not found");
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null) Debug.LogWarning("Animation not found");
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) Debug.LogWarning("AudioSource not found");
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
        bool isDestroyed = false;
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent<Player>(out var player)) player.Damage(1);
            isDestroyed = true;
        }
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null) _player.AddScore(10);
            isDestroyed = true;
        }
        if (isDestroyed)
        {
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
        }
    }
}
