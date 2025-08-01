using Assets.Define;
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
        stat = new MonsterStat(100, 10, 5f, 7f, 5f, 1);
        fsm.Init();
        col = GetComponent<Collider2D>();
        BattleManager.GetInstance.RegistHitInfo(GetComponent<Collider2D>(), Damaged);
        attackHandle = TypeFactory(attackType);
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

