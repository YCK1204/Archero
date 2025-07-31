using Assets.Define;
using Handler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Yoon.Handler
{
    class BossHandle : IAttackHandler
    {

        public void Init()
        {

        }


        public void AttackUpdate(int dmg, Vector3 dir, Vector3 target)
        {
            return;
        }

        public bool DelayCheck(float goal, float curr)
        {
            return goal <= curr;
        }
        //근접은 무조건 붙어야함
        public bool RangeCheck(float range, float dist)
        {
            return range >= dist;
        }

        public void OnCollision(Collider2D collider, int dmg, Vector3 dir)
        {
            BattleManager.GetInstance.Attack(collider, dmg, dir);
        }
        public IEnumerator OnCoroutine()
        {
            yield return null;
        }
    }
}
