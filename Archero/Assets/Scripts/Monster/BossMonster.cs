using Assets.Define;
using Handler;
using System.Collections;
using System.Collections.Generic;
using Unit.State;
using UnityEngine;
using UnityEngine.AI;

public class BossMonster : Monster
{
    // Start is called before the first frame update
    protected override void Start()
    {
        fsm = new MonsterStateMachine(GetComponent<Animator>());
        
        fsm.Init();
        col = GetComponent<Collider2D>();

        statSetter = new KingStatSetter();
        statSetter.StatChange(ref stat, 1);

        attackHandle = IAttackHandler.TypeFactory(MobType.Boss);
        moveHandler = IMoveHandler.Factory(MoveType.none,null);
        base.Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackHandle.DelayCheck(3f,attackTimer))
        {
            StartCoroutine(attackHandle.OnCoroutine(transform,target.position));
            attackTimer = 0f;
        }
    }
}

