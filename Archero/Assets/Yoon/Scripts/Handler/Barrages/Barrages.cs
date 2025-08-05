using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Handler.Barrages
{
    public abstract class Barrages
    {
        protected float shotDelay;//사격 딜레이
        public float ShetDelay() => shotDelay;
        protected float shotDuration;//사격 지속시간
        public float ShotDuration() => shotDuration;

        protected float angleGap;//z축을 기준
        protected float startAngle; //발사할 각도
        protected float defaultAngle;//초기화할 각도
        protected int damage;
        public int shotCount;//1회 사격시 발사 갯수
        public Barrages(float shotDelay, float shotDuration, float angleGap, float poviotAngle, int shotCount , int damage)
        {
            this.shotDelay = shotDelay;
            this.shotDuration = shotDuration;
            this.angleGap = angleGap;
            this.damage = damage;
            this.shotCount = shotCount;
        }
        public abstract void OnShot(Vector3 tr, float angle = 999f);
        public void Reset()
        {
            startAngle = defaultAngle;
        }
    }
}
