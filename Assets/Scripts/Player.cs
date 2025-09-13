using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, 0, 0);
        this._spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (this._spawnManager == null)
        {
            Debug.LogError("The SpawnManager not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > this._canFire)
        {
            this._canFire = Time.time + this._fireRate;
            Shoot();
        }
    }

    public void Damage(int damage)
    {
        this._lives -= damage;
        if (this._lives < 1)
        {
            this._spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    private void Shoot()
    {
        Vector3 laserPosition = new(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Instantiate(
            this._laserPrefab,
            laserPosition,
            Quaternion.identity
            );
    }

    private void CalculateMovement ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticallInput = Input.GetAxis("Vertical");

        Vector3 direction = new(horizontalInput, verticallInput, 0);

        transform.Translate(this._speed * Time.deltaTime * direction);


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
}
