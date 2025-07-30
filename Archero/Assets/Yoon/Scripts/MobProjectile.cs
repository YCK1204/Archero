using Assets.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobProjectile : MonoBehaviour
{
    Vector3 dir;
    Vector3 shooterPos;
    float speed;
    int damage;
    float currTime;
    public void Init(Vector3 dir,Vector3 shooterPos,float speed,int damage)
    {
        this.dir = dir;
        this.shooterPos = shooterPos;
        this.speed = speed;
        this.damage = damage;
        currTime = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= 5f) { BattleManager.GetInstance.normalMobProjectile.EnQueue(this);  return; }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BattleManager.GetInstance.normalMobProjectile.EnQueue(this);
            BattleManager.GetInstance.Attack(collision, damage, shooterPos);
        }
    }
}
