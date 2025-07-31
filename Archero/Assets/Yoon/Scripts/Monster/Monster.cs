using Assets.Define;
using Assets.Yoon.Handler;
using Handler;
using Handler.Barrages;
using System.Collections;
using System.Collections.Generic;
using Unit.State;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class Monster : MonoBehaviour
{
    protected IStateMachine fsm;
    protected MonsterStat stat;
    protected NavMeshAgent agent;
    [SerializeField]protected Transform target;

    protected IAttackHandler attackHandle;
    [SerializeField] protected MobType attackType;
    protected Collider2D col;

    protected float attackTimer = 0f;
    protected virtual void Start()
    {
        fsm = new MonsterStateMachine(GetComponent<Animator>());
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        stat = new MonsterStat(100,10,5f,7f,5f,1);
        agent.speed = stat.GetMoveSpeed;
        fsm.Init();
        col = GetComponent<Collider2D>();
        BattleManager.GetInstance.RegistHitInfo(GetComponent<Collider2D>(), Damaged);
        attackHandle = TypeFactory(attackType);
    }
    public void Init()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        BaseUpdate();
    }
    protected virtual bool BaseUpdate()
    {
        if (stat.isDie())
        {
            if (fsm.GetCurrentState != StateTypes.Die)
            {
                fsm.Chage(StateTypes.Die);
                BattleManager.GetInstance.monsterPool.EnQueue(this);
            }
            return false;
        }
        attackTimer = Mathf.Clamp(attackTimer + Time.deltaTime, 0, stat.GetAttackDelay);
        if (stat.GetDetectRange > GetDistance(transform.position, target.position))
        {
            if (attackHandle.RangeCheck(stat.GetAtkRange, GetDistance(transform.position, target.position)))
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                if (attackHandle.DelayCheck(stat.GetAttackDelay, attackTimer))
                {
                    attackHandle.AttackUpdate(stat.GetATK, transform.position, target.position);
                    attackTimer = 0f;
                }
                return false;
            }
            return false;
        }
        return true;
    }
    //루트계산 스킵,stat의 range들은 n을 입력시 생성자에서 n의 재곱으로 치환됨
    protected float GetDistance(Vector3 from, Vector3 to)
    {
        return Mathf.Pow(from.x - to.x, 2) +
            Mathf.Pow(from.y - to.y, 2);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (attackHandle.DelayCheck(stat.GetAttackDelay, attackTimer))
        {
            attackHandle.OnCollision(collision.collider,stat.GetATK, transform.position);
            attackTimer = 0f;
        }
    }
    public void Damaged(int damage,Vector3 attackerPos)
    {

        // 넉백 방향
        Vector3 direction = transform.position - attackerPos;
        Vector3 knockbackDir = direction.normalized;
        agent.velocity = knockbackDir * 3f;
        
        stat.GetDamage(damage);
        if(stat.isDie()) BattleManager.GetInstance.RemoveHitInfo(col);
    }

    public virtual void Spawn(MobType type , Vector3[] patrolPos , MonsterStat stat)
    {
        this.stat = stat;
        TypeFactory(type);
        fsm.ForceChange(StateTypes.Patrol);
    }

    private IAttackHandler TypeFactory(MobType type)
    {
        switch (type)
        {
            case MobType.Melee:
                return new MeleeHandle();
            case MobType.Ranged:
                return new RangeHandle();
            case MobType.Boss:
                return new BossHandle(new Handler.Barrages.Barrages[3]{new MultiShot(0.2f,2f,45f,90,8), new MultiShot(0.1f, 2f, 5f, 90, 8), new MultiShot(0.2f, 2f, 15f, 90, 3) });
        }
        return new MeleeHandle();
    }
}
