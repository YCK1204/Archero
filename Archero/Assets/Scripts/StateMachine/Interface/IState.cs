using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Unit.State
{
    interface IState
    {
        StateTypes GetStateType { get; }
        bool IsChangeAble();
        Animator anim { get; set; }
        void Enter();
        void Execute();
        void Exit();

    }
}
