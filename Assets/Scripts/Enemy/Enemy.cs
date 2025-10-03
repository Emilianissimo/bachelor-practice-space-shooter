using System.Collections;
using System.Collections.Generic;
using Perks;
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

    [SerializeField]
    private bool _evading = false;
    private float _evadingDirection;
    private bool _isDestroyed = false;

    private bool _isRammingPlayer = false;

    [SerializeField]
    private ShieldStatuses _shieldStatus = ShieldStatuses.Off;
    [SerializeField]
    private int _shieldStrength = 1;

    [SerializeField]
    private GameObject _shieldVisualiser;

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

        _randomX = Random.Range(-1f, 1f);

        SetShieldStatus(Random.Range(0, 2) == 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDestroyed) return;
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
        // Change position
        float gap = 0.2f;
        try
        {
            if (_player.transform.position.y > transform.position.y) gap = -2f;
        }
        catch (MissingReferenceException)
        {
            // Player is dead.
            return;
        }
            
        pos3.y -= gap;
        GameObject laser = Instantiate(
            _laserPrefab,
            pos3,
            Quaternion.identity
        );
        Laser[] lasers = laser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; ++i)
        {
            lasers[i].setIsEnemy(true);
            if (_player.transform.position.y > transform.position.y) lasers[i].setEnemyShootBackwards(true);
        }
        _audioSource.Play();
    }

    /// <summary>
    /// Calculating movement using field _speed. Loop on Y axis. Going down.
    /// </summary>
    private void CalculateMovement()
    {
        Vector3 direction;
        // We are gonna evade laser if value is on
        if (_evading)
        {
            direction = (Vector3.right * _evadingDirection).normalized;
            transform.Translate((_speed * 1.5f) * Time.deltaTime * direction);
            return;
        }

        if (_isRammingPlayer)
        {
            Vector3 playerPosition = _player.transform.position;
            direction = (playerPosition - transform.position).normalized;
            transform.Translate(_speed * Time.deltaTime * direction);
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= 1.5f) // once per 1.5 secs changing direction to random again
            _randomX = Random.Range(-1f, 1f);
        _timer = 0;
        direction = (Vector3.down + Vector3.right * _randomX).normalized;
        transform.Translate(_speed * Time.deltaTime * direction);

        float currentPositionY = transform.position.y;
        if (currentPositionY < -5)
        {
            transform.position = new Vector3(Random.Range(-11.5f, 11.5f), 5, 0);
        }
    }

    /// <summary>
    /// On collide space with laser try to evade it with delay
    /// </summary>
    public void EvadeLaser()
    {
        StartCoroutine(EvadeLaserCouroutine());
    }

    private IEnumerator EvadeLaserCouroutine()
    {
        _evading = true;
        _evadingDirection = Random.Range(-1f, 1f);
        yield return new WaitForSeconds(1);
        _evading = false;
    }

    public void SetRamPlayer(bool value)
    {
        _isRammingPlayer = value;
    }

    public void ShootPowerUp(PowerUp powerUp)
    {
        Shoot();
    }

    public void SetShieldStatus(bool on)
    {
        _shieldStatus = on ? ShieldStatuses.On : ShieldStatuses.Off;
        _shieldVisualiser.SetActive(on);
    }

    /// <summary>
    /// On collide with laser - destroy. On player - destroy & damage.
    /// </summary>
    /// <param name="other">Laser, Player</param>
    public void TriggerController(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent<Player>(out var player)) player.Damage(1);
            _isDestroyed = true;
        }
        if (other.CompareTag("Laser") || other.CompareTag("BigBoy"))
        {
            Destroy(other.gameObject);
            if (_player != null) _player.AddScore(10);
            _isDestroyed = true;
        }
        if (other.CompareTag("DeathRay"))
        {
            if (_player != null) _player.AddScore(10);
            _isDestroyed = true;
        }

        if (_isDestroyed)
        {
            if (_shieldStatus == ShieldStatuses.On)
            {
                _shieldStrength -= 1;
                if (_shieldStrength < 1)
                {
                    SetShieldStatus(false);
                    _isDestroyed = false;
                    return;
                }
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(this.gameObject, 2.8f);
        }
    }
}
