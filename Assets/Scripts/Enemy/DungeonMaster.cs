using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMaster : MonoBehaviour
{
    private SpawnManager _spawnManager;
    [SerializeField]
    private float _speed = 2f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private bool _immortal = true;

    [SerializeField]
    private float _fireRate = 1f;
    private float _canFire = -1f;

    private AudioSource _audioSource;

    private Player _player;

    private Animator _animator;

    private bool _isDestroyed = false;

    [SerializeField]
    private int _lives = 3;

    private bool _stopMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 7, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.LogWarning("Player not found");
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null) Debug.LogWarning("Animation not found");
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) Debug.LogWarning("AudioSource not found");
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("The SpawnManager not found");
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDestroyed && !_immortal) return;
        if (!_stopMovement) CalculateMovement();
        // Shoot only after stop
        if (Time.time > _canFire && _stopMovement)
        {
            Shoot();
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        if (transform.position.y < 3)
        {
            _stopMovement = true;
            _immortal = false;
        }
    }

    private void Shoot()
    {
        _canFire = Time.time + _fireRate;
        Vector3 pos3 = transform.position;
        pos3.y -= 3f;

        Instantiate(
            _laserPrefab,
            pos3,
            Quaternion.Euler(0, 0, -30f)
        );
        Instantiate(
            _laserPrefab,
            pos3,
            Quaternion.Euler(0, 0, -15f)
        );
        Instantiate(
            _laserPrefab,
            pos3,
            Quaternion.identity
        );
        Instantiate(
            _laserPrefab,
            pos3,
            Quaternion.Euler(0, 0, 15f)
        );
        Instantiate(
            _laserPrefab,
            pos3,
            Quaternion.Euler(0, 0, 30f)
        );

        _audioSource.Play();
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.Damage(3);
            _isDestroyed = true;
        }
        if (other.CompareTag("Laser") || other.CompareTag("BigBoy"))
        {
            Destroy(other.gameObject);
            if (_player != null) _player.AddScore(100);
            _isDestroyed = true;
        }
        if (other.CompareTag("DeathRay"))
        {
            if (_player != null) _player.AddScore(100);
            _isDestroyed = true;
        }

        if (_isDestroyed && !_immortal)
        {
            if (_lives > 1)
            {
                _lives--;
                _isDestroyed = false;
                return;
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
    
    private void OnDestroy()
    {
        if (_spawnManager != null) _spawnManager.OnBossDeath();
    }
}
