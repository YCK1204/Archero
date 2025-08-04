using Assets.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpDropItem : DropItem
{
    private int amount;
    public override void Init(int amount) 
    {
        this.amount = amount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerExpHandler>().GainExp(amount);
            Debug.LogError("나중에 수정해야함");
            //BattleManager.GetInstance.Items[0].EnQueue(this);
        }
    }
}
