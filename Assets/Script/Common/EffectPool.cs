using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    public static EffectPool effectPool;
    //�̸� ������ prefab�� Object ��
    [SerializeField] int bufferAmount = 20;
    //������ Object ��ϵ� ���� �迭
    [SerializeField] private GameObject[] effects;
    //������ Object���� ��� List���� ���� �迭
    private List<GameObject>[] pooledEffects;


    private void Awake()
    {
        if (effectPool == null)
        {
            effectPool = this;
        }
        //���� ������ ������ŭ ����Ʈ �迭 ����
        pooledEffects = new List<GameObject>[effects.Length];
        for (int i = 0; i < effects.Length; i++)
        {
            //����Ʈ �迭�� ��Ҹ���, ������ Prefab Object ������� List ���� �� �Ҵ�
            pooledEffects[i] = new List<GameObject>();
            //����Ʈ �ϳ�����, bufferAmount ����ŭ ������Ʈ ���� �� ��ҷ� �߰�
            for (int j = 0; j < bufferAmount; j++)
            {
                GameObject obj = Instantiate(effects[i], transform);
                obj.name = effects[i].name;//�̸� ��ȣ�ߴ� �� ����
                PoolObject(obj);
            }

        }
    }

    /// <summary>
    /// �Ʒ��� returnŸ���� ����, �Ű������� GameObject�� �޼ҵ��Դϴ�.
    /// </summary>
    /// <param name="obj">������ GameObject�� �ֽ��ϴ�.(name�̰��ƾ� �մϴ�)</param>
    /// <returns></returns>
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
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == TypeName)
            {
                //Prefab Object �����ϰ� �ִ� ����Ʈ ��� 1�� �̻��� �͸�
                if (pooledEffects[i].Count > 0)
                {
                    GameObject pooledObject = pooledEffects[i][0];
                    //������ Prefab Object �����ϰ� �ִ� ����Ʈ���� ��ȯ�� ��Ҹ� ����
                    pooledEffects[i][0].SetActive(true);
                    pooledEffects[i].RemoveAt(0);
                    return pooledObject;
                }
                else //����Ʈ�� �����ִ°� �ϳ��� ������ �� Object ����
                {
                    GameObject newObject = Instantiate(effects[i], transform);
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
        for (int i = 0; i < effects.Length; ++i)
        {
            if (effects[i].name == obj.name)
            {
                obj.SetActive(false);
                pooledEffects[i].Add(obj);
                return;
            }
        }
    }

}
