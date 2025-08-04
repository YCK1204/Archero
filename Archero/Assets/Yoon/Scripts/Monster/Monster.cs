using Assets.Define;
using Assets.Yoon.Handler;
using Handler;
using Handler.Barrages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unit.State;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class Monster : MonoBehaviour
{
    protected IStateMachine fsm;
    [SerializeField]protected MonsterStat stat;
    protected NavMeshAgent agent;
    [SerializeField]protected Transform target;

    protected IAttackHandler attackHandle;
    [SerializeField] protected MobType attackType;

    protected IMoveHandler moveHandler;
    [SerializeField]protected MoveType moveType;

    [SerializeField]private ChessCharType chessType;
    private IStatManaging statSetter;
    protected Collider2D col;

    protected float attackTimer = 0f;
    protected float playerDist = 0f;
    protected virtual void Start()
    {
        fsm = new MonsterStateMachine(GetComponent<Animator>());
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        statSetter = IStatManaging.Factory(stat, chessType);
        statSetter.StatChange(ref stat, 1);
        
        agent.speed = stat.GetMoveSpeed;
        fsm.Init();
        col = GetComponent<Collider2D>();
        attackHandle = IAttackHandler.TypeFactory(attackType);
        moveHandler = IMoveHandler.Factory(moveType,agent);
        //YOON : stage정보 넘겨주면 1대신 해당 인스턴스 넣어주면됨

        Init();
    }
    public void Init()
    {
        BattleManager.GetInstance.RegistHitInfo(col, Damaged);
        gameObject.SetActive(true);
        if(target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
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
                BattleManager.GetInstance.monsterPool[chessType].EnQueue(this);
                Lee.Scripts.GameManager.Instance.CheckStageClear();
            }
            return false;
        }
        playerDist = GetDistance(transform.position, target.position);
        attackTimer = Mathf.Clamp(attackTimer + Time.deltaTime, 0, stat.GetAttackDelay);
        if (stat.GetDetectRange > playerDist)
        {
            if (attackHandle.RangeCheck(stat.GetAtkRange, playerDist))
            {
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
        if (collision.gameObject.layer != 6) return;
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

    public virtual void Spawn(MobType type , Vector3[] patrolPos , MonsterStat stat,ChessCharType chessType)
    {
        this.stat = stat;

        fsm.ForceChange(StateTypes.Patrol);
        BattleManager.GetInstance.RegistHitInfo(col, Damaged);
        //YOON : stageNum 받아와야됨
        statSetter.StatChange(ref stat, /*stageNum*/1);
        Init();
    }


}
[Serializable]
public enum ChessCharType { pawn,knight,bishop,rock}
interface IStatManaging
{ 
    void ItemDrop(Vector3 pos);
    void StatChange(ref MonsterStat currStat,int stageNum);
    public static IStatManaging Factory(MonsterStat stat,ChessCharType type)
    {
        switch (type)
        {
            case ChessCharType.pawn:
                return new PawnStatSetter();
            case ChessCharType.knight:
                return new KnightStatSetter();
            case ChessCharType.bishop:
                return new BishopStatSetter();
            case ChessCharType.rock:
                return new RockStatSetter();
        }
        return null;
    }
}
public class PawnStatSetter:IStatManaging 
{
    public void ItemDrop(Vector3 pos)
    {
        DropItem item = BattleManager.GetInstance.Items[0].DeQueue();
        item.Init(10);
        item.transform.position = pos;
        if (new System.Random().Next(0, 100) > 90)
        {
            BattleManager.GetInstance.Items[1].DeQueue();
            item.Init(10);
        }
    }
    public void StatChange(ref MonsterStat stat, int stageNum)
    {
        int hp = 30 + (int)(0.5f + (0.5f * stageNum));
        float moveSpeed = 1f * (0.95f + (0.05f * stageNum));
        stat = new MonsterStat(hp, hp, moveSpeed, 7, 1, 0.5f);
    }
}
public class KnightStatSetter:IStatManaging 
{
    public void ItemDrop(Vector3 pos)
    {
        DropItem item = BattleManager.GetInstance.Items[0].DeQueue();
        item.Init(20);
        item.transform.position = pos;
        if (new System.Random().Next(0, 100) > 30)
        {
            BattleManager.GetInstance.Items[1].DeQueue();
            item.Init(10);
        }
    }
    public void StatChange(ref MonsterStat stat, int stageNum)
    {
        int hp = 50 + (int)(0.5f + (0.5f * stageNum));
        float moveSpeed = 1f * (0.90f + (0.01f * stageNum));
        stat = new MonsterStat(hp, hp, moveSpeed, 7, 1, 0.5f);
    }
}
public class BishopStatSetter:IStatManaging 
{
    public void ItemDrop(Vector3 pos)
    {
        DropItem item = BattleManager.GetInstance.Items[0].DeQueue();
        item.Init(20);
        item.transform.position = pos;
        if (new System.Random().Next(0, 100) > 30)
        {
            BattleManager.GetInstance.Items[1].DeQueue();
            item.Init(10);
        }
    }
    public void StatChange(ref MonsterStat stat, int stageNum)
    {
        int hp = 30 + (int)(0.5f + (0.5f * stageNum));
        int atk = (int)(hp * 0.7f);
        stat = new MonsterStat(hp, atk, 1, 5, 4, 0.5f);
    }
}
public class RockStatSetter:IStatManaging
{
    public void ItemDrop(Vector3 pos)
    {
        DropItem item = BattleManager.GetInstance.Items[0].DeQueue();
        item.Init(30);
        item.transform.position = pos;

        BattleManager.GetInstance.Items[1].DeQueue();
        item.Init(40);
    }
    public void StatChange(ref MonsterStat stat, int stageNum)
    {
        int hp = 60 + (int)(0.5f + (0.5f * stageNum));
        float moveSpeed = 2f * (0.95f + (0.05f * stageNum));
        stat = new MonsterStat(hp, hp, moveSpeed, 7, 4, (1.5f*(1.06f-(0.06f*stageNum))));
    }
}