using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    [SerializeField] float lifeTime = 5.0f;

    /// <summary>
    /// 사용법 익힐 것
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
