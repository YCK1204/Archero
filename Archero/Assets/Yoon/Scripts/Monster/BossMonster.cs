using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackHandle.DelayCheck(3f,attackTimer))
        {
            StartCoroutine(attackHandle.OnCoroutine(transform.position,target.position));
            attackTimer = 0f;
        }
    }
}

