using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Configuration")]
public class WaveConfig : ScriptableObject
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _pathPrefab;

    [SerializeField]
    private float _timeBetweenSpawn = 0.5f;

    [SerializeField]
    private float _spawnRandomFactor = 0.3f;

    [SerializeField]
    private int _noOfEnemies = 5;

    [SerializeField]
    private float _moveSpeed = 2f;

    [SerializeField]
    private string _enemyTypes = string.Empty;
    

    public GameObject GetEnemyPrefab()
    {
        return _enemyPrefab;
    }

    public List<Transform> GetWayPoints()
    {
        var waveWayPoints = new List<Transform>();

        foreach (Transform child in _pathPrefab.transform)
        {
            waveWayPoints.Add(child);
        }

        return waveWayPoints;
    }

    public float GetTimeBetweenSpawn()
    {
        return _timeBetweenSpawn;
    }

    public float GetSpawnRandomFactor()
    {
        return _spawnRandomFactor;
    }

    public int GetNoOfEnemies()
    {
        return _noOfEnemies;
    }

    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }

    public string GetEnemyTypes()
    {
        return _enemyTypes;
    }

    
}
