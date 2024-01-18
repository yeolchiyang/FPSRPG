using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichIceAttack : MonoBehaviour
{
    private float DamagedTime = 0f;
    /// <summary>
    /// 맞은 판정을 판단하는 collider
    /// </summary>
    private float iceDamage;
    private float EffectCountTime = 0f;
    [Tooltip("IceMagic 이펙트가 출력되고, 삭제되는데 걸리는 시간을 설정합니다.")]
    [SerializeField] private float EffectDestroyDelay = 2f;
    private ParticleSystem IceMagicParticle;


    public void SetIceDamage(float damage)
    {
        iceDamage = damage;
    }


    private void OnEnable()
    {
        IceMagicParticle = GetComponent<ParticleSystem>();
        IceMagicParticle.Play();
        EffectCountTime = Time.time;
        StartCoroutine(SpawnIceMagic());
    }

    IEnumerator SpawnIceMagic()
    {
        while (true)
        {
            if (EffectCountTime + EffectDestroyDelay <= Time.time)
            {
                IceMagicParticle.Stop();
                EffectPool.effectPool.PoolObject(gameObject);
                StopAllCoroutines();
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (DamagedTime + EffectDestroyDelay <= Time.time)
        {
            DamagedTime = Time.time;
            collision.gameObject.GetComponent<Player_Health>().TakeDamage(iceDamage);
            int restraint = 2;//기절을 의미
            collision.gameObject.GetComponent<Player_Health>().PlayerCcon(restraint);
        }

    }
}
