using Assets.Define;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Handler
{
    public class MeleeHandle : IAttackHandler
    {
        public void AttackUpdate(int dmg, Vector3 position, Vector3 target)
        {
            
        }

        public bool DelayCheck(float goal, float curr)
        {
            return goal <= curr;
        }
        //근접은 무조건 붙어야함
        public bool RangeCheck(float range, float dist)
        {
            return false; //range >= dist;
        }

        public void OnCollision(Collider2D collider, int dmg, Vector3 dir)
        {
            BattleManager.GetInstance.Attack(collider, dmg, dir);
        }

        public IEnumerator OnCoroutine(Vector3 firePos, Vector3 targetPos)
        {
            yield return null;
        }
    }
} 
