using Handler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    [SerializeField] Vector3[] patrolPositions;
    int patrolIndex = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {

        if (!BaseUpdate())
        {
            agent.SetDestination(target.transform.position);
            fsm.Chage(StateTypes.Trace);
            return; 
        }


        if (fsm.GetCurrentState != StateTypes.Patrol)
        {
            agent.ResetPath();
            fsm.Chage(StateTypes.Patrol);
        }
        PatrolLoop();
    }



    void OnDrawGizmosSelected()
    {
        if (patrolPositions == null || patrolPositions.Length <= 0) return;
        for (int i = 0; i < patrolPositions.Length; i++)
        {
            Gizmos.color = new Color(1f, 0f, 1f);
            Gizmos.DrawSphere(patrolPositions[i], 0.5f);
        }
    }

    public override void Spawn(MobType type, Vector3[] patrolPos, MonsterStat stat)
    {
        base.Spawn(type, patrolPos, stat);
        patrolPositions = patrolPos;
    }

    private void PatrolLoop()
    {
        if (patrolPositions.Length == 0 || patrolPositions == null) return;//패트롤 없으면 제자리 대기
        if (agent.remainingDistance < 0.1f)
        {
            agent.velocity = Vector3.zero;

            agent.SetDestination(patrolPositions[patrolIndex]);
            patrolIndex++;
            if (patrolIndex >= patrolPositions.Length) patrolIndex = 0;
        }
    }
}
