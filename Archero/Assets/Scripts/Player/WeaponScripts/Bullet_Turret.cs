using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Turret : Projectile
{
    public override void Init(Vector2 dir, WeaponData weaponData, int baseAttackPower)
    {
        // ���ݷ� 25%�� ����
        base.Init(dir, weaponData, Mathf.RoundToInt(baseAttackPower * 0.25f));


        Debug.Log("�ͷ� �Ѿ� �߻��!");
    }
}

