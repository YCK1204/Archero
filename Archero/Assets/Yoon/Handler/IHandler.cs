using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Handler
{
    public interface IAttackHandler
    {
        bool DelayCheck(float goal,float curr);
        bool RangeCheck(float range,float dist);
        void AttackUpdate(int dmg,UnityEngine.Vector3 dir,Vector3 target);
        void OnCollision(Collider2D collider,int dmg, Vector3 dir);
        System.Collections.IEnumerator OnCoroutine(Vector3 pos,Vector3 targetPos);
    }
    public enum MobType { Melee, Ranged ,Boss}
}
