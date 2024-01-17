using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichFireMagic : MonoBehaviour
{
    private float fireDamage;
    private float EffectCountTime = 0f;
    [SerializeField] private float EffectDestroyDelay = 2f;
    private ParticleSystem FireMagicParticle;
    /// <summary>
    /// ���� ������ �Ǵ��ϴ� collider
    /// </summary>
    private BoxCollider hitDetectionBox;
    /// <summary>
    /// ���� ������ Collider Box�� �̵� �ӵ�
    /// </summary>
    private const float HIT_DETECTION_SPEED = 12f;

    public void SetFireDamage(float damage)
    {
        fireDamage = damage;
    }


    private void OnEnable()
    {
        FireMagicParticle = GetComponent<ParticleSystem>();
        hitDetectionBox = GetComponent<BoxCollider>();
        FireMagicParticle.Play();
        EffectCountTime = Time.time;
        StartCoroutine(MoveFire());
    }

    IEnumerator MoveFire()
    {
        while (true)
        {
            hitDetectionBox.transform.Translate(transform.forward * Time.deltaTime * HIT_DETECTION_SPEED);
            if (EffectCountTime + EffectDestroyDelay <= Time.time)
            {
                FireMagicParticle.Stop();
                hitDetectionBox.transform.position = transform.position;
                EffectPool.effectPool.PoolObject(gameObject);
                StopAllCoroutines();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    

}
