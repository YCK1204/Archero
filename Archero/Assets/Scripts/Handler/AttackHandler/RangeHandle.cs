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
    public class RangeHandle : IAttackHandler
    {
        public void AttackUpdate(int dmg, Vector3 position,Vector3 target)
        {
            Vector2 tempVec = target - position;
            float rad = Mathf.Atan2(tempVec.y, tempVec.x);
            float degree = rad * (180f / MathF.PI);
            degree -= 90f;
            BattleManager.GetInstance.normalMobProjectile.DeQueue().
                Init(new Vector3(0,0,degree), position, 10f,dmg);
        }

        public bool DelayCheck(float goal, float curr)
        {
            return goal <= curr;
        }
        //근접은 무조건 붙어야함
        public bool RangeCheck(float range, float dist)
        {
            return range > dist;
        }

        public void OnCollision(Collider2D collider, int dmg, Vector3 dir)
        {
            
        }

        public IEnumerator OnCoroutine(Transform firePos, Vector3 targetPos)
        {
            yield return null;
        }
    }
} 
