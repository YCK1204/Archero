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
            stateDict.Add(StateTypes.Patrol, new PatrolState());
            stateDict.Add(StateTypes.Trace, new TraceState());
            stateDict.Add(StateTypes.Attack, new AttackState());
            stateDict.Add(StateTypes.Damaged, new DamagedState());
            stateDict.Add(StateTypes.Die, new DieState());
            ForceChange(StateTypes.Patrol);
        }
        public void Chage(StateTypes type)
        {
            if (currState.GetStateType == StateTypes.Die||
                currState.GetStateType == type) return;
            currState.Exit();
            currState = stateDict[type];
            currState.Enter();
        }

        private void ForceChange(StateTypes type)
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

