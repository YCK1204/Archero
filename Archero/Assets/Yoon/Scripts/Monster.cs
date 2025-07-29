using System.Collections;
using System.Collections.Generic;
using Unit.State;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    IStateMachine fsm;
    MonsterStat stat;
    NavMeshAgent agent;
    [SerializeField]Vector3[] patrolPositions;
    int patrolIndex = 0;
    [SerializeField]Transform target;
    void Start()
    {
        fsm = new MonsterStateMachine(GetComponent<Animator>());
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        stat = new MonsterStat(100,10,5f,3f,1f);
        agent.speed = stat.GetMoveSpeed;
        fsm.Init();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stat.GetDetectRange > GetDistance())
        {
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
        return Mathf.Abs(Mathf.Pow(target.position.x - transform.position.x, 2) -
            Mathf.Pow(target.position.y - transform.position.y, 2));
    }
    void OnDrawGizmosSelected()
    {
        
        for (int i = 0; i < patrolPositions.Length; i++)
        {
            Gizmos.color = new Color(1f, 0f, 1f);
            Gizmos.DrawSphere(patrolPositions[i],0.5f);
        }
    }
    private void PatrolLoop()
    {
        
        if (agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(patrolPositions[patrolIndex]);
            patrolIndex++;
            if (patrolIndex >= patrolPositions.Length) patrolIndex = 0;
        }
    }
}
