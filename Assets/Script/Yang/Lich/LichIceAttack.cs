using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichIceAttack : MonoBehaviour
{
    private float DamagedTime = 0f;
    /// <summary>
    /// ���� ������ �Ǵ��ϴ� collider
    /// </summary>
    private float iceDamage;
    private float EffectCountTime = 0f;
    [Tooltip("IceMagic ����Ʈ�� ��µǰ�, �����Ǵµ� �ɸ��� �ð��� �����մϴ�.")]
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
            int restraint = 2;//������ �ǹ�
            collision.gameObject.GetComponent<Player_Health>().PlayerCcon(restraint);
        }

    }
}
