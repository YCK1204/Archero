using Assets.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobProjectile : MonoBehaviour
{
    float speed;
    int damage;
    float currTime;

    public void Init(Vector3 rot,Vector3 shooterPos,float speed,int damage)
    {
        transform.eulerAngles = rot;
        transform.position = shooterPos;
        this.speed = speed;
        this.damage = damage;
        currTime = 0f;

        transform.position = shooterPos;
    }
    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= 2f) { BattleManager.GetInstance.normalMobProjectile.EnQueue(this);  return; }
        transform.position += transform.up * (Time.deltaTime*speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BattleManager.GetInstance.normalMobProjectile.EnQueue(this);
            BattleManager.GetInstance.Attack(collision, damage, transform.position);
        }
        else if (collision.gameObject.layer == 3) BattleManager.GetInstance.normalMobProjectile.EnQueue(this);
    }

}
