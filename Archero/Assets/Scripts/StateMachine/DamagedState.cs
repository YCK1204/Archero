using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Unit.State
{
    class DamagedState : IState
    {

        public bool IsChangeAble() => currTime >= goalTime;
        float currTime;
        float goalTime;
        public StateTypes GetStateType => StateTypes.Damaged;
        public Animator anim { get; set; }
        
        public DamagedState(Animator anim)
        {
            this.anim = anim;
            foreach (var clip in anim.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "Damaged")
                {
                    goalTime = clip.length;
                    break;
                }
            }
            currTime = 0f;
        }

        public void Enter()
        {
            anim.Play("Damaged");

        }

        public void Execute()
        {
            currTime += Time.deltaTime;
        }

        public void Exit()
        {
            currTime = 0f;
        }
    }
}
