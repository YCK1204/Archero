using Assets.Define;
using System;
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
            /*Vector2 tempVec = target - position;
            float rad = Mathf.Atan2(tempVec.y, tempVec.x);
            float degree = rad * (180f / MathF.PI);//각도가 아니라 주석처리*/
            Vector2 rel = target - position;
            rel = rel.normalized;

            BattleManager.GetInstance.normalMobProjectile.DeQueue().
                Init(rel.normalized, position, 10f,dmg);
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
    }
} 
