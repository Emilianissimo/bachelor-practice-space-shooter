using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(this._speed * Time.deltaTime * Vector3.up);
        // enough time to escape camera view with 5m/per sec
        Destroy(this.gameObject, 2);
    }
}
