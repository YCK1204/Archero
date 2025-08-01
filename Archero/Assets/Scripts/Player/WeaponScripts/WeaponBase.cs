using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponData weaponData;
    protected CharacterStats ownerStats;
    protected WeaponHolder holder;

    public WeaponData WeaponData => weaponData;
    public virtual void Init(WeaponData data, CharacterStats stats, WeaponHolder weaponHolder)
    {
        weaponData = data;
        ownerStats = stats;
        holder = weaponHolder;
    }

    /// <summary>
    /// WeaponHolder가 매 프레임 호출해서 무기가 알아서 동작하게 함.
    /// 투사체 발사, 범위공격, 회전 등 무기마다 내부에서 처리.
    /// </summary>
    public abstract void Activate();
}