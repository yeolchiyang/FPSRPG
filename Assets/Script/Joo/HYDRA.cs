using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected void SetWalk(int value)
    {
        if(value == 0)
            HydraAnimator.SetBool("Walk", false);
        else
            HydraAnimator.SetBool("Walk", true);
    }

    protected void SetRoar()
    {
        HydraAnimator.SetTrigger("Roar");
    }

    protected void SetHeavyHit()
    {
        HydraAnimator.SetTrigger("HeavyHit");
    }

    protected void SetGrab()
    {
        HydraAnimator.SetTrigger("Grab");
    }

    protected void SetThreehit()
    {
        HydraAnimator.SetTrigger("Threehit");
    }

    protected void SetFivehit()
    {
        HydraAnimator.SetTrigger("Fivehit");
    }

    protected void IsAttacked()
    {
        if (IsnormalTargetReached()) // 물리공격
        {
            player.GetComponent<Player_Health>().TakeDamage(stat.NormalDamage);
        }
        
        if(IsskillTargetReached())
        {
            player.GetComponent<Player_Health>().TakeDamage(stat.SkillDamage);
        }
    }

    bool IsNTargetReached = false;
    protected bool IsnormalTargetReached()  // 일반공격 사거리 계산
    {
        float distanceToPlayer = Vector3.Distance(
            player.transform.position, transform.position);
        if (distanceToPlayer <= stat.NormalAttackRange)
        {
            IsNTargetReached = true;
        }
        else
        {
            IsNTargetReached = false;
        }
        return IsNTargetReached;
    }

    bool IsSTargetReached = false;
    protected bool IsskillTargetReached()  // 스킬공격 사거리 계산
    {
        float distanceToPlayer = Vector3.Distance(
            player.transform.position, transform.position);
        if (distanceToPlayer <= stat.SkillattackRange && distanceToPlayer > stat.NormalAttackRange)
        {
            IsSTargetReached = true;
            IsNTargetReached = false;
        }
        else
        {
            IsSTargetReached = false;
        }
        return IsSTargetReached;
    }

    public virtual void SetDamaged(float damage)  // 맞을 때 animation 없음
    {
        stat.CurrentHp -= damage;
        if(stat.CurrentHp <= 0f)
        {
            stat.CurrentHp = 0f;
            HydraAnimator.SetBool("Death", true);
            StartCoroutine(Die());
        }
    }

    protected IEnumerator Die()
    {
        yield return new WaitForSeconds(2f);
        float timer = 0;
        float sinkingtime = 5f;

        while(true)
        {
            timer += Time.deltaTime;
            if(timer > sinkingtime)
            {
                transform.position += Vector3.down * Time.fixedDeltaTime * 0.5f;
                GetComponent<SphereCollider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = false;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
