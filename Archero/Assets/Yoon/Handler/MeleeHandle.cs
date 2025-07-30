using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Handler
{
    public class MeleeHandle : IAttackHandler
    {
        public void Attack(int dmg, Vector3 dir)
        {
            throw new NotImplementedException();
        }

        public bool DelayCheck(float goal, float curr)
        {
            throw new NotImplementedException();
        }

        public bool RangeCheck(float range, float dist)
        {
            throw new NotImplementedException();
        }
    }
} 
