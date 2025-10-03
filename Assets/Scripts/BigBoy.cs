using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    private GameObject _nearest = null;

    void Start()
    {
        // List of enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        // Finding nearest by just checking each
        foreach (GameObject e in enemies)
        {
            float dist = (e.transform.position - currentPos).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                _nearest = e;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        if (_nearest != null)
        {
            // Enemy direction
            Vector3 dir = (_nearest.transform.position - transform.position).normalized;

            // Movement
            transform.Translate(dir * _speed * Time.deltaTime);

            // // Rotation towards our object
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            return;
        }
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
    }
}
