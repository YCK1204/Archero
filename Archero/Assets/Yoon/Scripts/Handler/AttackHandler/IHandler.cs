using Assets.Yoon.Handler;
using Handler.Barrages;
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
        System.Collections.IEnumerator OnCoroutine(Transform pos,Vector3 targetPos);
        static IAttackHandler TypeFactory(MobType type)
        {
            switch (type)
            {
                case MobType.Melee:
                    return new MeleeHandle();
                case MobType.Ranged:
                    return new RangeHandle();
                case MobType.Boss:
                    return new BossHandle(new Handler.Barrages.Barrages[3] { new MultiShot(0.2f, 2f, 45f, 90f, 8), new MultiShot(0.1f, 2f, 5f, 90f, 8), new MultiShot(0.2f, 2f, 90f, 90f, 4) });
            }
            return new MeleeHandle();
        }
    }
    public enum MobType { Melee, Ranged ,Boss}
}
