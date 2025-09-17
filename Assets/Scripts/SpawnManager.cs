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
    [SerializeField]
    private GameObject _trippleShotPowerUpPrefab;
    private bool _spawning = true;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(EnemiesSpawnRoutine());
        StartCoroutine(PowerUpsSpawnRoutine());
    }

    public void OnPlayerDeath()
    {
        _spawning = false;
    }

    /// <summary>
    /// Spawning routine, spawns objects not related to user input;
    /// Enemies.
    /// </summary>
    /// <returns>Yield of delay to rerun</returns>
    private IEnumerator EnemiesSpawnRoutine()
    {
        while (_spawning)
        {
            Vector3 enemyPosition = new(Random.Range(-11.5f, 11.5f), 5, 0);
            GameObject newEnemy = Instantiate(
                _enemyPrefab,
                enemyPosition,
                Quaternion.identity
                );
            newEnemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(5.0f);
        }
    }

    /// <summary>
    /// Spawning routine, spawns objects not related to user input;
    /// PowerUps.
    /// </summary>
    /// <returns>Yield of random delay (3-7 secs) to rerun</returns>
    private IEnumerator PowerUpsSpawnRoutine()
    {
        while (_spawning)
        {
            Vector3 newPowerUPPosition = new(Random.Range(-11.5f, 11.5f), 5, 0);
            GameObject newPowerUP = Instantiate(
                _trippleShotPowerUpPrefab,
                newPowerUPPosition,
                Quaternion.identity
                );
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }
}
