using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit.State;
using UnityEngine;

namespace Unit.State
{
    class TraceState : IState
    {

        public bool IsChangeAble() => true;
        public StateTypes GetStateType => StateTypes.Trace;
        public Animator anim { get;set; }
        public TraceState(Animator anim)
        {
            this.anim = anim;
        }

        public void Enter()
        {
            anim.Play("Trace");
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
