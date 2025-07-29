using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit.State
{
    class DamagedState : IState
    {
        public StateTypes GetStateType => StateTypes.Damaged;

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
