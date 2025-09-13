using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _spawning = true;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    public void OnPlayerDeath()
    {
        this._spawning = false;
    }

    private IEnumerator SpawnRoutine()
    {
        while (_spawning)
        {
            Vector3 enemyPosition = new(Random.Range(-11.5f, 11.5f), 5, 0);
            GameObject newEnemy = Instantiate(
                this._enemyPrefab,
                enemyPosition,
                Quaternion.identity
                );
            newEnemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(5.0f);
        }
    }
}
