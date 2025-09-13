using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, 5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.CalculateMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.TryGetComponent<Player>(out var player))
            {
                player.Damage(1);
            }
            Destroy(this.gameObject);
        } 
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void CalculateMovement()
    {
        this.transform.Translate(this._speed * Time.deltaTime * Vector3.down);

        float currentPositionY = this.transform.position.y;
        if (currentPositionY < -5)
        {
            this.transform.position = new Vector3(Random.Range(-11.5f, 11.5f), 5, 0);
        }
    }
}
