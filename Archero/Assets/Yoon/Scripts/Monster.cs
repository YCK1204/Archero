using Assets.Define;
using Handler;
using System.Collections;
using System.Collections.Generic;
using Unit.State;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class Monster : MonoBehaviour
{
    protected IStateMachine fsm;
    MonsterStat stat;
    NavMeshAgent agent;
    [SerializeField]Vector3[] patrolPositions;
    [SerializeField]Transform target;
    
    IAttackHandler attackHandle;
    [SerializeField] MobType attackType;

    int patrolIndex = 0;
    float attackTimer = 0f;
    void Start()
    {
        fsm = new MonsterStateMachine(GetComponent<Animator>());
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        stat = new MonsterStat(100,10,5f,7f,5f,1);
        agent.speed = stat.GetMoveSpeed;
        fsm.Init();
        BattleManager.GetInstance.RegistHitInfo(GetComponent<Collider2D>(), Damaged);
        attackHandle = TypeFactory(attackType);
    }
    public void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (stat.isDie()) 
        {
            if (fsm.GetCurrentState != StateTypes.Die)
            {
                fsm.Chage(StateTypes.Die);
                BattleManager.GetInstance.monsterPool.EnQueue(this);
            }
            return;
        }
        attackTimer = Mathf.Clamp(attackTimer+Time.deltaTime,0,stat.GetAttackDelay);
        if (stat.GetDetectRange > GetDistance())
        {
            if (attackHandle.RangeCheck(stat.GetAtkRange, GetDistance()))
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                if (attackHandle.DelayCheck(stat.GetAttackDelay, attackTimer))
                {
                    attackHandle.AttackUpdate(stat.GetATK, transform.position,target.position);
                    attackTimer = 0f;
                }
                return;
            }

            agent.SetDestination(target.transform.position);
            fsm.Chage(StateTypes.Trace);
        }
        else
        {
            if(fsm.GetCurrentState != StateTypes.Patrol)
            {
                agent.ResetPath();
                fsm.Chage(StateTypes.Patrol);
            }
            PatrolLoop();
        }
    }
    //루트계산 스킵,stat의 range들은 n을 입력시 생성자에서 n의 재곱으로 치환됨
    private float GetDistance()
    {
        return Mathf.Pow(target.position.x - transform.position.x, 2) +
            Mathf.Pow(target.position.y - transform.position.y, 2);
    }
    void OnDrawGizmosSelected()
    {
        
        for (int i = 0; i < patrolPositions.Length; i++)
        {
            Gizmos.color = new Color(1f, 0f, 1f);
            Gizmos.DrawSphere(patrolPositions[i],0.5f);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (attackHandle.DelayCheck(stat.GetAttackDelay, attackTimer))
        {
            attackHandle.OnCollision(collision.collider,stat.GetATK, transform.position);
            attackTimer = 0f;
        }
    }

    private void PatrolLoop()
    {
        if (patrolPositions.Length == 0) return;//패트롤 없으면 제자리 대기
        if (agent.remainingDistance < 0.1f)
        {
            agent.velocity = Vector3.zero;

            agent.SetDestination(patrolPositions[patrolIndex]);
            patrolIndex++;
            if (patrolIndex >= patrolPositions.Length) patrolIndex = 0;
        }
    }
    public void Damaged(int damage,Vector3 attackerPos)
    {

        // 넉백 방향
        Vector3 direction = transform.position - attackerPos;
        Vector3 knockbackDir = direction.normalized;
        agent.velocity = knockbackDir * 3f;
        stat.GetDamage(damage);
    }

    public void Spawn(MobType type , Vector3[] patrolPos , MonsterStat stat)
    {
        patrolPositions = patrolPos;
        this.stat = stat;
        TypeFactory(type);
        fsm.ForceChange(StateTypes.Patrol);
    }

    private IAttackHandler TypeFactory(MobType type)
    {
        switch (type)
        {
            case MobType.melee:
                return new MeleeHandle();
            case MobType.Ranged:
                return new RangeHandle();

        }
        return new MeleeHandle();
    }
}
