using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float _fireRate = 3f;
    private float _canFire = -1f;

    private AudioSource _audioSource;

    private Player _player;

    private Animator _animator;
    private float _randomX;
    private float _timer = 0f;

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

        _randomX = Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Shooting methodm can be runned only in case of passed cooldown (_canFire).
    /// Setting new cooldown using the _fireRate value which is random between 3 and 7 secs;
    /// Instantiates lasers according to current powerup.
    /// </summary>
    private void Shoot()
    {
        _fireRate = Random.Range(3f, 7f);
        _canFire = Time.time + _fireRate;
        Vector3 pos3 = transform.position;
        pos3.y -= 0.2f;
        GameObject laser = Instantiate(
            _laserPrefab,
            pos3,
            Quaternion.identity
        );
        Laser[] lasers = laser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; ++i)
        {
            lasers[i].setIsEnemy(true);
        }
        _audioSource.Play();
    }

    /// <summary>
    /// Calculating movement using field _speed. Loop on Y axis. Going down.
    /// </summary>
    private void CalculateMovement()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1.5f) // once per 1.5 secs changing direction to random again
            _randomX = Random.Range(-1f, 1f);
        _timer = 0;
        Vector3 direction = (Vector3.down + Vector3.right * _randomX).normalized;
        transform.Translate(_speed * Time.deltaTime * direction);

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

        if (other.CompareTag("DeathRay"))
        {
            if (_player != null) _player.AddScore(10);
            isDestroyed = true;
        }

        if (isDestroyed)
        {
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
