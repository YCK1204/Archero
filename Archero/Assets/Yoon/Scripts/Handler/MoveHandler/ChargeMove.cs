using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
namespace Handler
{
    public class ChargeMove : IMoveHandler
    {
        bool castingCharging = false;
        float chargeTime;
        Transform tr;
        NavMeshAgent agent;
        public ChargeMove(NavMeshAgent ag) {agent = ag; tr = ag.transform; }
        public bool GetMoveCondition(float currDist, float dist)
        {
            return currDist <= dist;
        }

        public void OnMove(Vector3 dir, float speed)
        {
            Vector3 arrivePos = dir;
            float dist = Vector3.Distance(arrivePos, tr.position);
            tr.DOKill(true);
            tr.DOMove(arrivePos, dist*chargeTime).OnComplete(() => 
            {
                agent.Warp(tr.position); 
            });
            castingCharging = false;
        }

        public bool Timer(float currTime, float goalTime)
        {
            if (!castingCharging) 
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                chargeTime = goalTime / 20f;
            }
            castingCharging = true;
            return currTime >= goalTime;
        }
    }
}
