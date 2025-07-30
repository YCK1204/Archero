using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Handler
{
    interface IAttackHandler
    {
        bool DelayCheck(float goal,float curr);
        bool RangeCheck(float range,float dist);
        void Attack(int dmg,UnityEngine.Vector3 dir);
    }
}
