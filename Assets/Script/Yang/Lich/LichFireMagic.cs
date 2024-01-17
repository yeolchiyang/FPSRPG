using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichFireMagic : MonoBehaviour
{
    private float fireDamage;
    private float EffectCountTime;
    [SerializeField] private float EffectDestroyDelay = 2f;

    public void SetFireDamage(float damage)
    {
        fireDamage = damage;
    }


    private void OnEnable()
    {
        EffectCountTime = Time.time;
        StartCoroutine(MoveFire());
    }

    IEnumerator MoveFire()
    {
        while (true)
        {
            Debug.Log("Fire½ÇÇàÁß");
            transform.Translate(transform.forward * Time.deltaTime);
            if(EffectCountTime + EffectDestroyDelay >= Time.time)
            {
                EffectPool.effectPool.PoolObject(gameObject);
                StopAllCoroutines();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    

}
