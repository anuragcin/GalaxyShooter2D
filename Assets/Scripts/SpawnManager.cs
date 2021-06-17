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

    [SerializeField]
    private List<WaveConfig> _waveConfig;

    public void StartSpawning()
    {
        
        StartCoroutine(SpawnAllWaves()); //Spawn All New Waves..

        //StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    /// <summary>
    /// Spawn All Waves
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnAllWaves()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log(_waveConfig.Count);
        while (_stopSpawning == false)
        {
            int randomEnemyWave = ChooseEnemyWave();
            var currentWave = _waveConfig[randomEnemyWave];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));

            yield return new WaitForSeconds(5.0f);
        }
    }

    /// <summary>
    /// Spawn All Enemies In Wave
    /// </summary>
    /// <param name="waveConfig"></param>
    /// <returns></returns>
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {

        for (int enemyCount=0; enemyCount < waveConfig.GetNoOfEnemies(); enemyCount++)
        {
            //Instantiate an GameObject-EnemyPrefab
            GameObject newEmeny = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWayPoints()[0].transform.position,
                Quaternion.identity);
            newEmeny.GetComponent<Enemy>().SetWaveConfig(waveConfig);
            newEmeny.SetActive(true);

            newEmeny.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawn());
        }
     
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

            int randomPowerUp = ChoosePowerUp();
            Instantiate(_powerUps[randomPowerUp], spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3,8));

        }
    }

    public void OnPlayerDead()
    {
        _stopSpawning = true;
    }


    /// <summary>
    /// Choose PowerUp
    /// </summary>
    /// <returns></returns>
    private int ChoosePowerUp()
    {
        int _weightedTotal = 0;

        int[] powerupTable =
        {
            60,
            45,
            20,
            10,
            8,
            6,
            2
        };

        int[] powerupToAward =
        {
            3,
            5,
            4,
            2,
            1,
            0,
            6
        };

        foreach (var item in powerupTable)
        {
            _weightedTotal += item;
        }

        int randomNumber = Random.Range(0, _weightedTotal);
        var i = 0;

        foreach (var weight in powerupTable)
        {
            if (randomNumber <= weight)
            {
                return powerupToAward[i];
            }
            else
            {
                i++;
                randomNumber -= weight;
            }
        }
        return powerupToAward[i];
    }

    /// <summary>
    /// Choose Enemy..
    /// </summary>
    /// <returns></returns>
    private int ChooseEnemyWave()
    {
        int _weightedTotal = 0;

        int[] enemyWaveTable =
        {
            90,
            20,
            50
        };

        int[] enemyWaveToAward =
        {
            2, //SmartEnemy
            1, //Beta
            0 //Alpha
        };

        foreach (var item in enemyWaveTable)
        {
            _weightedTotal += item;
        }

        int randomNumber = Random.Range(0, _weightedTotal);
        var i = 0;

        foreach (var weight in enemyWaveTable)
        {
            if (randomNumber <= weight)
            {
                return enemyWaveToAward[i];
            }
            else
            {
                i++;
                randomNumber -= weight;
            }
        }

        return enemyWaveToAward[i];

    }
}