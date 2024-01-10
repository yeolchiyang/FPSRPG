using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner objectSpawner;
    [Tooltip("스폰할 위치입니다. 멤버변수로 집어넣은 게임 오브젝트의 위치에 잡몹이 소환됩니다.")]
    [SerializeField] private GameObject[] spawnPoints;
    [Tooltip("스폰할 오브젝트를 집어넣습니다")]
    [SerializeField] private GameObject[] enemys;
    [Tooltip("리젠 주기 입니다.")]
    [SerializeField] private float regenDelay = 5f;
    [Tooltip("리젠 제한 일반 몬스터 수 입니다.")]
    [SerializeField] private int regenLimitedCount = 30;
    [Tooltip("현재 소환된 일반 몬스터 수 입니다.")]
    [SerializeField] private int currentSpawnedCount = 0;
    private bool isSpawning = true;


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
            if (isSpawning)
            {
                regenCount += Time.deltaTime;
                if (regenCount >= regenDelay)
                {
                    if (CurrentSpawnedCount <= RegenLimitedCount)
                    {
                        SpawnRandomPoints();
                        CurrentSpawnedCount++;
                    }
                    regenCount = 0f;
                }
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
    /// <summary>
    /// 스폰을 멈춥니다. 동시에 소환되어 있는 일반 몬스터를 사망처리 합니다.
    /// </summary>
    public void StopSpawning()
    {
        this.isSpawning = false;
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemys)
        {
            if (enemy.GetComponent<NormalSkeleton>().isActive)
            {
                enemy.GetComponent<NormalSkeleton>().SetDie();
            }
        }
    }
    /// <summary>
    /// 스폰을 시작합니다.
    /// </summary>
    public void StartSpawning()
    {
        this.isSpawning = true;
    }
}
