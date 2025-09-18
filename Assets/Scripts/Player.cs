using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Perks;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedIncreased = 2;
    [SerializeField]
    private float _speedDecreased = -2;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _trippleLaserPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    [SerializeField]
    private ShootingModes _shootingMode = ShootingModes.SingleShot;
    private Dictionary<ShootingModes, GameObject> _shootingModes;

    [SerializeField]
    private SpeedModes _speedMode = SpeedModes.Default;
    private Dictionary<SpeedModes, float> _speedModes;

    [SerializeField]
    private ShieldStatuses _shieldStatus = ShieldStatuses.Off;

    [SerializeField]
    private GameObject _shieldVisualiser;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;

    private UI_Manager _UIManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null) Debug.LogError("The SpawnManager not found");
        _UIManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_UIManager == null) Debug.LogError("The UIManager not found");

        _UIManager.SetLives(_lives);

        // Initializing ShootingModes map, it will need to be done here
        // Optimized type of perks in hash map and easy to modify and extend
        _shootingModes = new Dictionary<ShootingModes, GameObject>
        {
            {ShootingModes.SingleShot, _laserPrefab },
            {ShootingModes.TrippleShot, _trippleLaserPrefab }
        };

        _speedModes = new Dictionary<SpeedModes, float>
        {
            {SpeedModes.Default, 1 },
            {SpeedModes.Increased, _speedIncreased },
            {SpeedModes.Decreased, _speedDecreased },
        };
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Shooting methodm can be runned only in case of passed cooldown (_canFire).
    /// Setting new cooldown using the _fireRate value;
    /// Instantiates lasers according to current powerup.
    /// </summary>
    private void Shoot()
    {
        _canFire = Time.time + _fireRate;
        Vector3 pos3 = transform.position;
        if (_shootingMode == ShootingModes.SingleShot)
        {
            pos3.y += 0.8f;
        }
        Instantiate(
            _shootingModes[_shootingMode],
            pos3,
            Quaternion.identity
        );
    }

    /// <summary>
    /// Calculating movement using field _speed. 
    /// Moving by reading input from WASD on horizontal and vertical axis.
    /// Y axis is limited, X axis is cycled.
    /// Speed works according to the current boost/downgrade which is collected
    /// </summary>
    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticallInput = Input.GetAxis("Vertical");

        Vector3 direction = new(horizontalInput, verticallInput, 0);

        transform.Translate(
            (_speed *
            _speedModes[_speedMode]) *
            Time.deltaTime *
            direction
        );


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, min: -4.5f, max: 3.6f), 0);

        float currentPositionX = transform.position.x;
        if (currentPositionX >= 11.5f)
        {
            transform.position = new Vector3(-11.5f, transform.position.y, 0);
        }
        else if (currentPositionX <= -11.5f)
        {
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }
    }

    /// <summary>
    /// Damaging player on amount of damage we provided.
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    public void Damage(int damage)
    {
        if (_shieldStatus == ShieldStatuses.On)
        {
            _shieldStatus = ShieldStatuses.Off;
            _shieldVisualiser.SetActive(false);
            return;
        }
        _lives -= damage;
        _UIManager.SetLives(_lives);

        if (_lives == 2){
            _leftEngine.SetActive(true);
        }else if(_lives == 1){
            _rightEngine.SetActive(true);
        }
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
            _UIManager.SetGameOverState(true);
        }
    }

    /// <summary>
    /// Collecting shooting powerup
    /// </summary>
    /// <param name="mode">ShootingModes enum value</param>
    public void CollectPowerShootingUp(ShootingModes mode)
    {
        if (_shootingModes.ContainsKey(mode))
        {
            _shootingMode = mode;
            StartCoroutine(TrippleShotPowerDownRoutine());
        }
    }

    /// <summary>
    /// Routine to disable powerup after 5 seconds
    /// </summary>
    /// <returns>yield sleep for 5 sec</returns>
    private IEnumerator TrippleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _shootingMode = ShootingModes.SingleShot;
    }

    /// <summary>
    /// Collecting speed multiplier type
    /// </summary>
    /// <param name="mode">ShootingModes enum value</param>
    public void CollectPowerSpeedUpOrDown(SpeedModes mode)
    {
        if (_speedModes.ContainsKey(mode))
        {
            _speedMode = mode;
            StartCoroutine(SpeedMultiplierPowerDownRoutine());
        }
    }

    /// <summary>
    /// Routine to disable speed multiplier after 5 seconds
    /// </summary>
    /// <returns>yield sleep for 5 sec</returns>
    private IEnumerator SpeedMultiplierPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedMode = SpeedModes.Default;
    }

    /// <summary>
    /// Collecting Shield PowerUP setting it On for logic
    /// and setting on visualiser
    /// </summary>
    public void CollectPowerShield()
    {
        _shieldStatus = ShieldStatuses.On;
        _shieldVisualiser.SetActive(true);
    }

    /// <summary>
    /// Method to manipulate score from other objects
    /// </summary>
    /// <param name="score">Maybe both negative and positive integers</param>
    public void AddScore(int score)
    {
        if (this._score < 1 && score < 1)
        {
            return;
        }
        this._score += score;
        _UIManager.SetScore(this._score);
    }
}
