using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.State
{
    public class MonsterStateMachine : IStateMachine
    {
        private Dictionary<StateTypes, IState> stateDict;
        private IState currState;
        public StateTypes GetCurrentState => currState.GetStateType;
        private Animator anim;
        public MonsterStateMachine(Animator anim)
        {
            this.anim = anim;
        }

        public void Init()
        {
            if (stateDict != null) { ForceChange(StateTypes.Patrol); return; }
            
            stateDict = new Dictionary<StateTypes, IState>(5);
            stateDict.Add(StateTypes.Patrol, new PatrolState(anim));
            stateDict.Add(StateTypes.Trace, new TraceState(anim));
            stateDict.Add(StateTypes.Attack, new AttackState(anim));
            stateDict.Add(StateTypes.Damaged, new DamagedState(anim));
            stateDict.Add(StateTypes.Die, new DieState(anim));
            ForceChange(StateTypes.Patrol);
        }
        public void Chage(StateTypes type)
        {
            //죽거나 같은 상태 진입 혹은 변경이 가능하지 않을때
            if (currState.GetStateType == StateTypes.Die || currState.GetStateType == type || !currState.IsChangeAble()) return;
            currState.Exit();
            currState = stateDict[type];
            currState.Enter();
        }

        public void ForceChange(StateTypes type)
        {
            currState = stateDict[type];
            currState.Enter();
        }

        public void Update()
        {
            currState.Execute();
        }
    }
}

