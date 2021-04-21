using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _stopSpawning = false;

    [SerializeField]
    private List<GameObject> _powerUps;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    /// <summary>
    ///Spawn Game Object every 5 seconds
    ///Create a Co-routine of type IEnumerator that will yield events
    ///While Loop
    ///instantiate a game object -enemy prefab
    ///yield for 5 secons
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(5.0f);// breathing room for spawning objects..

        while (_stopSpawning == false)
        {
            //Random spawnPosition from -11 to 11 on x-axis, 7 on Y-axis and 0 on z-axis
            Vector3 spawnPosition = new Vector3(Random.Range(-11.0f, 11.0f), 7, 0);

            //Instantiate an GameObject-EnemyPrefab
            GameObject newEmeny = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEmeny.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(5.0f);
        }
    }

    /// <summary>
    /// Spawning TripleShot after 7 sec 
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            //Random spawnPosition from -11 to 11 on x-axis, 7 on Y-axis and 0 on z-axis
            Vector3 spawnPosition = new Vector3(Random.Range(-11.0f, 11.0f), 7, 0);
            int randomPowerUp = Random.Range(0, 3);
            //Instantiate an GameObject-EnemyPrefab
            Instantiate(_powerUps[randomPowerUp], spawnPosition, Quaternion.identity);
            
            yield return new WaitForSeconds(Random.Range(3,8));

        }
    }

    public void OnPlayerDead()
    {
        _stopSpawning = true;
    }
}
