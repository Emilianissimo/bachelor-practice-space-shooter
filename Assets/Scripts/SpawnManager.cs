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

    [SerializeField]
    private List<GameObject> _powerUps;

    public void StartSpawning()
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
        yield return new WaitForSeconds(2.0f);
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
        yield return new WaitForSeconds(5.0f);
        while (_spawning)
        {
            GameObject powerUp = _powerUps[Random.Range(0, _powerUps.Count)];
            Vector3 newPowerUPPosition = new(Random.Range(-11.5f, 11.5f), 5, 0);
            Instantiate(
                powerUp,
                newPowerUPPosition,
                Quaternion.identity
                );
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }
}
