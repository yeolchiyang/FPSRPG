using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HYDRA : MonoBehaviour
{
    protected bool isActive = true;
    protected HYDRAStat stat;
    protected UnityEngine.AI.NavMeshAgent Hydra;
    protected GameObject player;
    [SerializeField] protected Animator HydraAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        Hydra = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        stat = GetComponent<HYDRAStat>();
        HydraAnimator = GetComponent<Animator>();
    }

    public void SetWalk(bool value)
    {
        HydraAnimator.SetBool("Walk", value);
    }

    public void SetRoar()
    {
        HydraAnimator.SetTrigger("Roar");
    }

    public void SetHeavyHit()
    {
        HydraAnimator.SetTrigger("HeavyHit");
    }

    public void SetThreehit()
    {
        HydraAnimator.SetTrigger("Threehit");
    }

    public void SetFivehit()
    {
        HydraAnimator.SetTrigger("Fivehit");
    }

    //public void IsAttacked()
    //{
    //    if (IsnormalTargetReached()) // 물리공격
    //    {
    //        player.GetComponent<Player_Health>().TakeDamage(stat.NormalDamage);
    //    }

    //    if(IsskillTargetReached())
    //    {
    //        player.GetComponent<Player_Health>().TakeDamage(stat.SkillDamage);
    //    }
    //}

    //bool IsNTargetReached = false;
    //public bool IsnormalTargetReached()  // 일반공격 사거리 계산
    //{
    //    float distanceToPlayer = Vector3.Distance(
    //        player.transform.position, transform.position);
    //    if (distanceToPlayer <= stat.NormalAttackRange)
    //    {
    //        IsNTargetReached = true;
    //    }
    //    else
    //    {
    //        IsNTargetReached = false;
    //    }
    //    return IsNTargetReached;
    //}

    //bool IsSTargetReached = false;
    //public bool IsskillTargetReached()  // 스킬공격 사거리 계산
    //{
    //    float distanceToPlayer = Vector3.Distance(
    //        player.transform.position, transform.position);
    //    if (distanceToPlayer <= stat.SkillattackRange && distanceToPlayer > stat.NormalAttackRange)
    //    {
    //        IsSTargetReached = true;
    //        IsNTargetReached = false;
    //    }
    //    else
    //    {
    //        IsSTargetReached = false;
    //    }
    //    return IsSTargetReached;
    //}

    public void SetDamaged(float damage)  // 맞을 때 animation 없음
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer < stat.SkillattackRange)
        {
            stat.CurrentHp -= damage;
            if (stat.CurrentHp <= 0f)
            {
                stat.CurrentHp = 0f;
                if (isActive)
                {
                    HydraAnimator.SetTrigger("Death");
                }
                isActive = false;

                StartCoroutine(Die());
            }
        }
    }

    protected IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);
        float timer = 0;
        float sinkingtime = 5f;

        while(true)
        {
            timer += Time.deltaTime;

            GetComponent<SphereCollider>().enabled = false;
            //GetComponent<Rigidbody>().isKinematic = false;
            SphereCollider[] colist = GetComponentsInChildren<SphereCollider>();
            for (int i = 0; i < colist.Length; ++i)
            {
                colist[i].enabled = false;
            }

            //if (timer > sinkingtime)
            //{
            //    transform.position += Vector3.down * Time.fixedDeltaTime * 0.5f;
            //    break;
            //}

            //yield return new WaitForSeconds(5f);
            //GetComponent<NavMeshAgent>().enabled = false;

            yield return new WaitForFixedUpdate();
        }
    }

    public bool IsActivecheck()
    {
        return isActive;
    }
}
