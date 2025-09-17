using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 5f;

    /// <summary>
    /// Rinning new laser and destroy laser and its parent if it exists after 2 seconds (which is enough with speed 5m per sec
    /// </summary>
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
        Destroy(gameObject, 2);
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject, 2);
        }
    }
}
