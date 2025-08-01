using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData weaponData;
    protected float lastAttackTime;
    public WeaponData WeaponData => weaponData;

    public virtual void Init(WeaponData data)
    {
        weaponData = data;
        lastAttackTime = -Mathf.Infinity;
    }

    public virtual void TickAttack()
    {
        if (Time.time - lastAttackTime >= weaponData.Cooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    protected abstract void PerformAttack();
}