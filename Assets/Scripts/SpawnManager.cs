using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _dronKillerPrefab;

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _trippleShotPowerUpPrefab;
    private bool _spawning = true;

    [SerializeField]
    private List<GameObject> _powerUps;
    [SerializeField]
    private List<float> _weights;

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
            Vector3 enemyPosition = new(Random.Range(-11.5f, 11.5f), 7, 0);
            GameObject newEnemy = Instantiate(
                _enemyPrefab,
                enemyPosition,
                Quaternion.identity
            );
            Vector3 dronKillerPosition = new(Random.Range(-11.5f, 11.5f), 7, 0);
            GameObject newDronKiller = Instantiate(
                _dronKillerPrefab,
                dronKillerPosition,
                Quaternion.identity
            );
            newEnemy.transform.SetParent(_enemyContainer.transform);
            newDronKiller.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(5.0f);
        }
    }

    private GameObject ChoosePowerUpByWeight()
    {
        float total = 0f;
        foreach (float w in _weights) total += w;
        float rand = Random.value * total;
        float accum = 0f;
        int index = 0;
        for (int i = 0; i < _weights.Count; i++)
        {
            accum += _weights[i];
            if (rand <= accum) { index = i; break; }
        }
        return _powerUps[index];
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
            GameObject powerUp =  ChoosePowerUpByWeight();
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
