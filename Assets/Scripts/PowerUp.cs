using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Perks;

public enum PowerUps
{
    TrippleShot,
    SpeedMultiplier,
    Shield,
    Ammo,
    FirstAid,
    DeathRay,
}

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private PowerUps _powerUpID;

    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private AudioClip _collectSound;

    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) Debug.LogWarning("Player not found");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    /// <summary>
    /// Calculating movement using field _speed. Destroy object on passing bottom line.
    /// </summary>
    void CalculateMovement()
    {
        // If C pressed, move towards the player
        if (Input.GetKey(KeyCode.C))
        {
            Vector3 playerPosition = _player.transform.position;
            // Normalizing position. Without that we will get absolute and incorrect position for our purpose
            Vector3 direction = (playerPosition - transform.position).normalized;
            transform.Translate(_speed * 2 * Time.deltaTime * direction);
            return;
        }
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        float currentPositionY = transform.position.y;
        if (currentPositionY < -5)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Triggering on collission entered. Collecting this powerup by player. Desroying the object of param other is Player
    /// </summary>
    /// <param name="other">
    /// An actual object which collided with current
    /// </param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent<Player>(out var player))
            {
                switch (_powerUpID)
                {
                    case PowerUps.TrippleShot:
                        player.CollectPowerShootingUp(ShootingModes.TrippleShot);
                        break;
                    case PowerUps.SpeedMultiplier:
                        SpeedModes[] options = { SpeedModes.Increased, SpeedModes.Decreased };
                        SpeedModes value = options[Random.Range(0, options.Length)];
                        player.CollectPowerSpeedUpOrDown(value);
                        break;
                    case (PowerUps.Shield):
                        player.CollectPowerShield();
                        break;
                    case (PowerUps.Ammo):
                        player.CollectAmmo(5);
                        break;
                    case (PowerUps.FirstAid):
                        player.CollectFirstAid();
                        break;
                    case (PowerUps.DeathRay):
                        player.CollectPowerShootingUp(ShootingModes.DeathRay);
                        break;
                    default:
                        break;
                }
            }
            AudioSource.PlayClipAtPoint(_collectSound, transform.position);
            Destroy(this.gameObject);
        }
        if (other.CompareTag("Laser"))
        {
            AudioSource.PlayClipAtPoint(_collectSound, transform.position);
            Destroy(this.gameObject);
        }
    }
}
