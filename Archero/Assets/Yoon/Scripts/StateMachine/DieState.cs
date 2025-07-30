using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.State
{
    public class DieState : IState
    {
        public StateTypes GetStateType => StateTypes.Die;
        public Animator anim { get; set; }
        public bool IsChangeAble() => false;
        public DieState(Animator anim)
        {
            this.anim = anim;
        }

        public void Enter()
        {
            anim.Play("DieState");
        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }
    }
}

