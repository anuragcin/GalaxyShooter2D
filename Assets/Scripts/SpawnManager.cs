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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }



    /// <summary>
    ///Spawn Game Object every 5 seconds
    ///Create a Co-routine of type IEnumerator that will yield events
    ///While Loop
    ///instantiate a game object -enemy prefab
    ///yield for 5 secons
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnRoutine()
    {
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

    public void OnPlayerDead()
    {
        _stopSpawning = true;
    }
}
