using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichFireMagic : MonoBehaviour
{
    private float fireDamage;
    private float EffectCountTime = 0f;
    private float DamagedTime = 0f;
    [SerializeField] private float EffectDestroyDelay = 2f;
    private ParticleSystem FireMagicParticle;
    /// <summary>
    /// ���� ������ �Ǵ��ϴ� collider
    /// </summary>
    private BoxCollider hitDetectionBox;
    /// <summary>
    /// ���� ������ Collider Box�� �̵� �ӵ�
    /// </summary>
    private const float HIT_DETECTION_SPEED = 1f;
    private Vector3 fireDirection;


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
        fireDirection = FireMagicParticle.main.startSpeedMultiplier * FireMagicParticle.transform.forward;
    }

    IEnumerator MoveFire()
    {
        while (true)
        {
            hitDetectionBox.transform.Translate(fireDirection * Time.deltaTime * HIT_DETECTION_SPEED);
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

    private void OnCollisionEnter(Collision collision)
    {
        if(DamagedTime + EffectDestroyDelay <= Time.time)
        {
            DamagedTime = Time.time;
            collision.gameObject.GetComponent<Player_Health>().TakeDamage(fireDamage);
            int restraint = 2;//������ �ǹ�
            collision.gameObject.GetComponent<Player_Health>().PlayerCcon(restraint);
        }
        
    }


}
