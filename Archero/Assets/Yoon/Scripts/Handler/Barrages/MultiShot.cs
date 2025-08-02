using Assets.Define;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Handler.Barrages
{
    public class MultiShot : Barrages
    {
        
        public MultiShot(float shotDelay, float shotDuration, float angleGap, float pivotAngle,int shotCount) : base(shotDelay, shotDuration, angleGap, pivotAngle,shotCount)
        {
            this.shotDelay = shotDelay;
            this.shotDuration = shotDuration;
            this.angleGap = angleGap;
            this.shotCount = shotCount;

            defaultAngle = pivotAngle - (((float)shotCount / 2f)*angleGap);
            startAngle = defaultAngle;
        }

        public override void OnShot(Vector3 tr,float angle = 999f)
        {
            if (angle != 999f) { startAngle = angle - (((float)shotCount / 2f) * angleGap); }
            for (int i = 0; i < shotCount; i++)
            {
                startAngle += angleGap;
                BattleManager.GetInstance.normalMobProjectile.DeQueue().Init(new Vector3(0,0,startAngle),tr,10f,damage);
            }
            Reset();
        }
    }
}
