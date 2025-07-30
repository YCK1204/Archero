using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : WeaponBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private WeaponHolder holder;
    private void Awake()
    {
        holder = GetComponentInParent<WeaponHolder>();
    }
    public override void Init(WeaponData data)
    {
        base.Init(data);

    }
    protected override void PerformAttack()
    {
        Transform target = holder.FindNearestMonster(weaponData.Range);
        if (target == null) return;

        Vector2 baseDir = (target.position - firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        int count = weaponData.ProjectileCount;
        float spread = weaponData.SpreadAngle;
        float startAngle = baseAngle - spread * (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + spread * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
            proj.GetComponent<Projectile>()?.Init(dir, weaponData);
        }
    }
}
