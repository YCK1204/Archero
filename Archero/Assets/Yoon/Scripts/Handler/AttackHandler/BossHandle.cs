using Assets.Define;
using Handler;
using Handler.Barrages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

namespace Assets.Yoon.Handler
{
    class BossHandle : IAttackHandler
    {
        Barrages[] barrages;

        public BossHandle(Barrages[] barrages)
        {
            this.barrages = barrages;
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
        public IEnumerator OnCoroutine(Transform firePos,Vector3 targetPos)
        {
            Barrages currBarrage = barrages[new System.Random().Next(0, barrages.Length)];
            float totalTime = 0f;
            float fireTime = 0f;

            float duration = currBarrage.ShotDuration();
            float delay = currBarrage.ShetDelay();

            while (totalTime < currBarrage.ShotDuration())
            {
                totalTime += Time.deltaTime;
                fireTime += Time.deltaTime;

                if (fireTime >= delay)
                {


                    float rad = Mathf.Atan2(targetPos.y - firePos.position.y, targetPos.x - firePos.position.x);
                    float angle = rad*(180f/Mathf.PI);
                    angle -= 90f;
                    currBarrage.OnShot(firePos.position,angle);
                    fireTime = 0f;
                    currBarrage.Reset();
                }

                yield return null;
            }
        }
    }
}
