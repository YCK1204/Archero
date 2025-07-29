using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit.State;

namespace Unit.State
{
    class TraceState : IState
    {
        public StateTypes GetStateType => StateTypes.Trace;

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
