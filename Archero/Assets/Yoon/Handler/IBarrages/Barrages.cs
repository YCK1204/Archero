using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handler.Barrages
{
    public abstract class Barrages
    {
        protected float shotDelay;//사격 딜레이
        protected float shotDuration;//사격 지속시간
        protected float angleGap;//z축을 기준
        public int shotCount;//사격 횟수
        public Barrages(float shotDelay, float shotDuration, float angleGap, float poviotAngle, int shotCount)
        {
            this.shotDelay = shotDelay;
            this.shotDuration = shotDuration;
            this.angleGap = angleGap;
            this.shotCount = shotCount;
        }

        public abstract void OnShot();
    }
}
