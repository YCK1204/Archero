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
    /// WeaponHolder�� �� ������ ȣ���ؼ� ���Ⱑ �˾Ƽ� �����ϰ� ��.
    /// ����ü �߻�, ��������, ȸ�� �� ���⸶�� ���ο��� ó��.
    /// </summary>
    public abstract void Activate();
}