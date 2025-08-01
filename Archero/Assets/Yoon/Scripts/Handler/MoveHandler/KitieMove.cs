using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Handler
{
    public class KiteMove : IMoveHandler
    {
        NavMeshAgent agent;
        public KiteMove(NavMeshAgent agent)
        {
            this.agent = agent;
        }

        public bool GetMoveCondition(float currDist, float dist)
        {
            return currDist-(currDist/10f) < dist;
        }

        public void OnMove( Vector3 dir, float speed)
        {
            dir = dir * -1f;
            agent.SetDestination(dir);
        }

        public bool Timer(float currTime, float goalTime)
        {
            return true;
        }
    }
}
