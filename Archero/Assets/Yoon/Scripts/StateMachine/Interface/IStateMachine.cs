using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine
{
    StateTypes GetCurrentState { get; }
    void Init();
    void Update();
    void Chage(StateTypes type);
}
public enum StateTypes { Patrol,Die,Attack,Damaged,Trace}