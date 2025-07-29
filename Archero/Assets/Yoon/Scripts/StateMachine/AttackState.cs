using System.Collections;
using System.Collections.Generic;
using Unit.State;
using UnityEngine;
namespace Unit.State
{
    public class AttackState : IState
    {
        public StateTypes GetStateType => StateTypes.Attack;


        public bool IsChangeAble() => currTime >= goalTime;
        float currTime;
        float goalTime;
        public Animator anim { get; set; }

        public AttackState(Animator anim)
        {
            this.anim = anim;
            foreach (var clip in anim.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "Attack")
                {
                    goalTime = clip.length;
                    break;
                }
            }
            currTime = 0f;
        }

        public void Enter()
        {
            anim.Play("Attack");
        }

        public void Execute()
        {
            currTime += Time.deltaTime;
        }

        public void Exit()
        {
            currTime = 0;
        }
    }
}

