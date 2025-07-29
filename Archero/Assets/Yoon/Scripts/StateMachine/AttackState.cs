using System.Collections;
using System.Collections.Generic;
using Unit.State;
using UnityEngine;
namespace Unit.State
{
    public class AttackState : IState
    {
        public StateTypes GetStateType => StateTypes.Attack;

        public void Enter()
        {
        }

        public void Execute()
        {
        }

        public void Exit()
        {
        }
    }
}

