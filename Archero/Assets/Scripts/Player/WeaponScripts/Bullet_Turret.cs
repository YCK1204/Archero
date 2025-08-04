using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Turret : Projectile
{
    public override void Init(Vector2 dir, WeaponData weaponData, int baseAttackPower, Pool<Projectile> returnPool)
    {
        // °ø°Ý·Â 25%¸¸ Àû¿ë
        base.Init(dir, weaponData, Mathf.RoundToInt(baseAttackPower * 0.3f), returnPool);
        Debug.Log("ÅÍ·¿ ÃÑ¾Ë ¹ß»çµÊ!");
    }
}

