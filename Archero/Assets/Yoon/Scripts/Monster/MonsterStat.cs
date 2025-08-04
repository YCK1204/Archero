using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MonsterStat
{
    [SerializeField]private int currHP;
    [SerializeField] private int maxHP;
    [SerializeField] private int attackDamage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackDelay;
    [SerializeField] private float detectRange;
    [SerializeField] private float attackRange;
    public int GetATK { get { return attackDamage; } }
    public float GetMoveSpeed { get { return moveSpeed; } }
    public float GetDetectRange { get { return detectRange; } }
    public float GetAttackDelay { get { return attackDelay; } }

    public float GetAtkRange { get { return attackRange; } }

    public bool isDie() => currHP <= 0;
    public MonsterStat(int maxHP,int attackDamage,float moveSpeed,float detRange,float atkRange,float attackDelay) 
    {
        this.currHP = maxHP;
        this.maxHP = maxHP;
        this.attackDamage = attackDamage;
        this.moveSpeed = moveSpeed;
        detectRange = detRange * detRange;
        attackRange = atkRange * atkRange;
        this.attackDelay = attackDelay;
    }
    public void GetDamage(int damage)
    {
        if (isDie()) return;
        currHP -= damage;
    }
}
