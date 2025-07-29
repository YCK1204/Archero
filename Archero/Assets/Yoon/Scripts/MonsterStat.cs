using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat
{
    private int currHP;
    private int maxHP;
    private int attackDamage;
    private float moveSpeed;
    private float detectRange;
    public float GetMoveSpeed { get { return moveSpeed; } }
    public float GetDetectRange { get { return detectRange; } }
    private float attackRange;
    public float GetAtkRange { get { return attackRange; } }

    public bool isDie() => currHP <= 0;
    public MonsterStat(int maxHP,int attackDamage,float moveSpeed,float detRange,float atkRange) 
    {
        this.currHP = maxHP;
        this.maxHP = maxHP;
        this.attackDamage = attackDamage;
        this.moveSpeed = moveSpeed;
        detectRange = detRange * detRange;
        attackRange = atkRange * atkRange;
    }
    public void GetDamage(int damage)
    {
        if (isDie()) return;
        currHP -= damage;
    }
}
