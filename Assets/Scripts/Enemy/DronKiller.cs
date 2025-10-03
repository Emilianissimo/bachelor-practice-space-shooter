using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronKiller : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private AudioSource _audioSource;

    private Player _player;

    private Animator _animator;

    private float _directionX = -1;
    private bool _isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Reverse movement depending on initial position
        transform.position = new Vector3(Random.Range(-12f, 12f), 7, 0);
        if (transform.position.x < 0) _directionX = 1;

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
        if (_isDestroyed) return;
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        Vector3 direction;
        direction = (Vector3.down + Vector3.right * _directionX).normalized;
        transform.Translate(_speed * Time.deltaTime * direction);

        float currentPositionY = transform.position.y;
        if (currentPositionY < -5)
        {
            transform.position = new Vector3(Random.Range(-11.5f, 11.5f), 5, 0);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
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
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
