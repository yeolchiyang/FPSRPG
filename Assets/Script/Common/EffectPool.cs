using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    public static EffectPool effectPool;
    //미리 생성할 prefab당 Object 수
    [SerializeField] int bufferAmount = 20;
    //생성할 Object 목록들 담을 배열
    [SerializeField] private GameObject[] effects;
    //생성한 Object들을 담는 List들을 담을 배열
    private List<GameObject>[] pooledEffects;


    private void Awake()
    {
        if (effectPool == null)
        {
            effectPool = this;
        }
        //넣은 프리팹 개수만큼 리스트 배열 생성
        pooledEffects = new List<GameObject>[effects.Length];
        for (int i = 0; i < effects.Length; i++)
        {
            //리스트 배열의 요소마다, 생성할 Prefab Object 집어넣을 List 선언 후 할당
            pooledEffects[i] = new List<GameObject>();
            //리스트 하나마다, bufferAmount 수만큼 오브젝트 생성 후 요소로 추가
            for (int j = 0; j < bufferAmount; j++)
            {
                GameObject obj = Instantiate(effects[i], transform);
                obj.name = effects[i].name;//이름 괄호뜨는 것 방지
                PoolObject(obj);
            }

        }
    }

    /// <summary>
    /// 아래와 return타입이 같고, 매개변수만 GameObject인 메소드입니다.
    /// </summary>
    /// <param name="obj">꺼내올 GameObject를 넣습니다.(name이같아야 합니다)</param>
    /// <returns></returns>
    public GameObject GetObject(GameObject obj)
    {
        return GetObject(obj.name);
    }

    /// <summary>
    /// 활성화될 Object만 objectPool의 List에서 제거합니다.
    /// 오브젝트를 꺼내고자 하는 클래스에서 GetObject메소드에 
    /// 꺼내올 GameObject를 매개변수로 넣거나, 
    /// GameObject 의 이름(string)을 넣으면 됩니다.
    /// </summary>
    /// <param name="TypeName">꺼내올 Prefab Object Name입니다.</param>
    /// <returns></returns>
    public GameObject GetObject(string TypeName)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == TypeName)
            {
                //Prefab Object 저장하고 있는 리스트 요소 1개 이상인 것만
                if (pooledEffects[i].Count > 0)
                {
                    GameObject pooledObject = pooledEffects[i][0];
                    //생성한 Prefab Object 저장하고 있는 리스트에서 반환할 요소만 제거
                    pooledEffects[i][0].SetActive(true);
                    pooledEffects[i].RemoveAt(0);
                    return pooledObject;
                }
                else //리스트에 남아있는게 하나도 없으면 새 Object 생성
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
    /// 생성한 Object들을 담는 List(=pooledObjects[i])에 추가하는 함수
    /// Awake 시, 각 List마다 bufferAmount 수만큼 실행됩니다.
    /// Object를 파괴하고자 하는 클래스에서, 
    /// PoolObject메소드에 파괴하고자 하는 Object를 호출하면 됩니다.
    /// </summary>
    /// <param name="obj">파괴하고자 하는 Object를 넣습니다.</param>
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
