using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Turret : Projectile
{
    public override void Init(Vector2 dir, WeaponData weaponData, int baseAttackPower, Pool<Projectile> returnPool)
    {
        // 공격력 25%만 적용
        base.Init(dir, weaponData, Mathf.RoundToInt(baseAttackPower * 0.3f), returnPool);
    }
}

