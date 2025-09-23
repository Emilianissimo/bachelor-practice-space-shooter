using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Perks;

public enum PowerUps
{
    TrippleShot,
    SpeedMultiplier,
    Shield
}

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private PowerUps _powerUpID;

    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private AudioClip _collectSound;

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
                    default:
                        break;
                }
            }
            AudioSource.PlayClipAtPoint(_collectSound, transform.position);
            Destroy(this.gameObject);
        }
    }
}
