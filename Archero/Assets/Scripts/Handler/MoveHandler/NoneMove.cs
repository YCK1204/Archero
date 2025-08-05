using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Handler
{
    public class NoneMove : IMoveHandler
    {
        NavMeshAgent agent;
        public NoneMove(NavMeshAgent aget) { agent = aget; }
        public bool GetMoveCondition(float currDist, float dist)
        {
            return true;
        }

        public void OnMove(Vector3 dir, float speed)
        {
            return;
        }

        public bool Timer(float currTime, float goalTime)
        {
            return true;
        }
    }
}
