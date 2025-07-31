using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handler.Barrages
{
    public class MultiShot : Barrages
    {
        float startAngle;
        public MultiShot(float shotDelay, float shotDuration, float angleGap, float pivotAngle) : base(shotDelay, shotDuration, angleGap, pivotAngle)
        {
            this.shotDelay = shotDelay;
            this.shotDuration = shotDuration;
            this.angleGap = angleGap;
            
            startAngle = 
        }

        public override void OnShot()
        {
            
        }
    }
}
