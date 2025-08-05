using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Handler
{
    public class TraceMove : IMoveHandler
    {
        public NavMeshAgent agent;
        public TraceMove(NavMeshAgent agt) { agent = agt; }
        public bool GetMoveCondition(float currDist, float dist)
        {
            return currDist < dist;
        }

        public void OnMove(Vector3 dir, float speed)
        {
            agent.SetDestination(dir);
        }

        public bool Timer(float currTime, float goalTime)
        {
            return true;
        }
    }
}
