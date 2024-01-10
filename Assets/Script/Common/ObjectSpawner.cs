using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner objectSpawner;
    [Tooltip("������ ��ġ�Դϴ�. ��������� ������� ���� ������Ʈ�� ��ġ�� ����� ��ȯ�˴ϴ�.")]
    [SerializeField] private GameObject[] spawnPoints;
    [Tooltip("������ ������Ʈ�� ����ֽ��ϴ�")]
    [SerializeField] private GameObject[] enemys;
    [Tooltip("���� �ֱ� �Դϴ�.")]
    [SerializeField] private float regenDelay = 5f;
    [Tooltip("���� ���� �Ϲ� ���� �� �Դϴ�.")]
    [SerializeField] private int regenLimitedCount = 30;
    [Tooltip("���� ��ȯ�� �Ϲ� ���� �� �Դϴ�.")]
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
    /// ������ ����ϴ�. ���ÿ� ��ȯ�Ǿ� �ִ� �Ϲ� ���͸� ���ó�� �մϴ�.
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
    /// ������ �����մϴ�.
    /// </summary>
    public void StartSpawning()
    {
        this.isSpawning = true;
    }
}
