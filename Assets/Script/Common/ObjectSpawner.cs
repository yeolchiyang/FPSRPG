using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner objectSpawner;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private float regenDelay = 5f;
    [SerializeField] private int regenLimitedCount = 30;
    [SerializeField] private int currentSpawnedCount = 0;

    public int RegenLimitedCount
    {
        get { return regenLimitedCount; }
        set { regenLimitedCount = value; }
    }
    public int CurrentSpawnedCount
    {  
        get { return currentSpawnedCount; } 
        set { currentSpawnedCount = value; }
    }

    private void Awake()
    {
        if(objectSpawner == null )
        {
            objectSpawner = this;
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemys());
    }

    IEnumerator SpawnEnemys()
    {
        float regenCount = 0f;
        while (true)
        {
            regenCount += Time.deltaTime;
            if(regenCount >= regenDelay)
            {
                if (CurrentSpawnedCount <= RegenLimitedCount)
                {
                    SpawnRandomPoints();
                    CurrentSpawnedCount++;
                }
                regenCount = 0f;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void SpawnRandomPoints()
    {
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        int enemysIndex = Random.Range(0, enemys.Length);
        GameObject spawnObject = ObjectPool.objectPool.GetObject(enemys[enemysIndex]);
        spawnObject.transform.position = spawnPoints[randomSpawnPointIndex].transform.position;
    }

}
