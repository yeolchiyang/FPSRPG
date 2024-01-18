using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichIceCircle : MonoBehaviour
{
    private float iceDamage;
    private float EffectCountTime = 0f;
    [Tooltip("이펙트가 생긴 뒤, 몇초 뒤에 Ice Magic을 생성할 지 지정합니다.")]
    [SerializeField] private float EffectDestroyDelay = 3f;
    [SerializeField] private GameObject LichIceMagic;
    private ParticleSystem IceCircleParticle;


    public void SetIceDamage(float damage)
    {
        iceDamage = damage;
    }


    private void OnEnable()
    {
        IceCircleParticle = GetComponent<ParticleSystem>();
        IceCircleParticle.Play();
        EffectCountTime = Time.time;
        StartCoroutine(SpawnIceMagic());
    }

    IEnumerator SpawnIceMagic()
    {
        while (true)
        {
            if (EffectCountTime + EffectDestroyDelay <= Time.time)
            {
                IceCircleParticle.Stop();
                GameObject iceMagicObject = EffectPool.effectPool.GetObject(LichIceMagic);
                iceMagicObject.GetComponent<LichIceAttack>().SetIceDamage(iceDamage);
                iceMagicObject.transform.position = transform.position;
                EffectPool.effectPool.PoolObject(gameObject);
                StopAllCoroutines();
            }
            yield return new WaitForEndOfFrame();
        }
    }
    
}
