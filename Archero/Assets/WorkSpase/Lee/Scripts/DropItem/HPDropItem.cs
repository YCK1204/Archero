using Assets.Define;
using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDropItem : DropItem
{
    int healValue;
    public override void Init(int v)
    {
        healValue = v;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<CharacterStats>().Heal(healValue);
            BattleManager.GetInstance.Items[1].EnQueue(this);
        }
    }
}
