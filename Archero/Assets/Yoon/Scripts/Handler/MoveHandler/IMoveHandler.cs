using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Handler
{
    public interface IMoveHandler    
    {
        bool GetMoveCondition(float currDist,float dist);
        public void OnMove(Vector3 dir,float speed);
        public bool Timer(float currTime,float goalTime);
        public static IMoveHandler Factory(MoveType types,NavMeshAgent agent)
        {
            switch (types)
            {
                case MoveType.none:
                    return new NoneMove(agent);
                case MoveType.trace:
                    return new TraceMove(agent);
                case MoveType.charge:
                    return new ChargeMove(agent);
                case MoveType.kiting:
                    return new KiteMove(agent);
                default:
                    return new NoneMove(agent);
            }
        }
    }
    public enum MoveType { none,trace, charge,kiting}
}

