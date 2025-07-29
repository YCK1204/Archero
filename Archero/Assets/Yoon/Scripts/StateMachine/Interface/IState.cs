using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit.State
{
    interface IState
    {
        StateTypes GetStateType { get; }
        void Enter();
        void Execute();
        void Exit();

    }
}
