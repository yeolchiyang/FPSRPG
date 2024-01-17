using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Yang;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool objectPool;
    //�̸� ������ prefab�� Object ��
    [SerializeField] int bufferAmount = 20;
    //������ Object ��ϵ� ���� �迭
    [SerializeField] private GameObject[] prefabs;
    //������ Object���� ��� List���� ���� �迭
    private List<GameObject>[] pooledObjects;
    private int EliminatedEliteCount = 0;


    private void Awake()
    {
        if (objectPool == null)
        {
            objectPool = this;
        }
        //���� ������ ������ŭ ����Ʈ �迭 ����
        pooledObjects = new List<GameObject>[prefabs.Length];
        for(int i = 0; i < prefabs.Length; i++)
        {
            //����Ʈ �迭�� ��Ҹ���, ������ Prefab Object ������� List ���� �� �Ҵ�
            pooledObjects[i] = new List<GameObject>();
            //����Ʈ �ϳ�����, bufferAmount ����ŭ ������Ʈ ���� �� ��ҷ� �߰�
            for(int j = 0; j < bufferAmount; j++)
            {
                GameObject obj = Instantiate(prefabs[i], transform);
                obj.name = prefabs[i].name;//�̸� ��ȣ�ߴ� �� ����
                PoolObject(obj);
            }

        }
    }

    /// <summary>
    /// �Ʒ��� returnŸ���� ����, �Ű������� GameObject�� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>������ GameObject�� �ֽ��ϴ�.(name�̰��ƾ� �մϴ�)</returns>
    public GameObject GetObject(GameObject obj)
    {
        return GetObject(obj.name);
    }

    /// <summary>
    /// Ȱ��ȭ�� Object�� objectPool�� List���� �����մϴ�.
    /// ������Ʈ�� �������� �ϴ� Ŭ�������� GetObject�޼ҵ忡 
    /// ������ GameObject�� �Ű������� �ְų�, 
    /// GameObject �� �̸�(string)�� ������ �˴ϴ�.
    /// </summary>
    /// <param name="TypeName">������ Prefab Object Name�Դϴ�.</param>
    /// <returns></returns>
    public GameObject GetObject(string TypeName)
    {
        for ( int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].name == TypeName)
            {
                //Prefab Object �����ϰ� �ִ� ����Ʈ ��� 1�� �̻��� �͸�
                if (pooledObjects[i].Count > 0)
                {
                    GameObject pooledObject = pooledObjects[i][0];
                    //������ Prefab Object �����ϰ� �ִ� ����Ʈ���� ��ȯ�� ��Ҹ� ����
                    pooledObjects[i][0].SetActive(true);
                    pooledObjects[i].RemoveAt(0);
                    return pooledObject;
                }
                else //����Ʈ�� �����ִ°� �ϳ��� ������ �� Object ����
                {
                    GameObject newObject = Instantiate(prefabs[i], transform);
                    newObject.name = TypeName;
                    PoolObject(newObject);
                    newObject.SetActive(true);
                    return newObject;
                }
            }

        }
        return null;
    }

    /// <summary>
    /// ������ Object���� ��� List(=pooledObjects[i])�� �߰��ϴ� �Լ�
    /// Awake ��, �� List���� bufferAmount ����ŭ ����˴ϴ�.
    /// Object�� �ı��ϰ��� �ϴ� Ŭ��������, 
    /// PoolObject�޼ҵ忡 �ı��ϰ��� �ϴ� Object�� ȣ���ϸ� �˴ϴ�.
    /// </summary>
    /// <param name="obj">�ı��ϰ��� �ϴ� Object�� �ֽ��ϴ�.</param>
    public void PoolObject(GameObject obj)
    {
        for (int i = 0; i < prefabs.Length; ++i)
        {
            if (prefabs[i].name == obj.name)
            {
                obj.SetActive(false);
                pooledObjects[i].Add(obj);
                return;
            }
        }
    }
    /// <summary>
    /// �Ű��������� �̷��� ¥�� �ȵ�����.. ������ ī��Ʈ�� ������Ű�� ���� �Ű����� �Դϴ�.
    /// </summary>
    /// <param name="skeleton">Skeleton�� �⺻������ Player ������Ʈ�� ���ϰ� �ֽ��ϴ�.</param>
    public void IncrementBossRoomCount(Skeleton skeleton)
    {
        
        EliminatedEliteCount++;
        ObjectSpawner.objectSpawner.StartSpawning();
        if (EliminatedEliteCount == 2)
        {
            skeleton.PlayerObject.GetComponent<Player_Health>().BossHunting = true;
        }
    }

}
