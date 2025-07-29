using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.State
{
    public class PatrolState : IState
    {

        public bool IsChangeAble() => true;
        public StateTypes GetStateType => StateTypes.Patrol;
        public Animator anim { get; set; }
        public PatrolState(Animator anim)
        {
            this.anim = anim;
        }

        public void Enter()
        {
            anim.Play("PatrolState");

        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}
