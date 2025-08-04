using Assets.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeapon : WeaponBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private float lastAttackTime = -Mathf.Infinity;


    public override void Activate()
    {
        Debug.Log("TurretWeapon: Activate() 진입 시도");
        if (Time.time - lastAttackTime < weaponData.Cooldown / ownerStats.TotalStats.AttackSpeed)
        {
            Debug.Log("TurretWeapon: 쿨타임 미도래로 발사 취소");
            return;
        }

        Transform target = holder.FindNearestMonster(weaponData.Range);
        if (target == null)
        {
            Debug.Log("TurretWeapon: 대상 없음으로 발사 취소");
            return;
        }

        FireAt(target.position);
        lastAttackTime = Time.time;
    }

    private void FireAt(Vector2 targetPos)
    {
        Debug.Log("TurretWeapon: 발사 시도");
        Vector2 baseDir = (targetPos - (Vector2)firePoint.position).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        int count = weaponData.ProjectileCount;
        float spread = weaponData.SpreadAngle;
        float startAngle = baseAngle - spread * (count - 1) / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + spread * i;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            // 풀에서 꺼내기
            Projectile proj = BattleManager.GetInstance.turretProjectilePool.DeQueue();

            // 위치 및 회전
            proj.transform.position = firePoint.position;
            proj.transform.rotation = Quaternion.Euler(0, 0, angle);
            proj.gameObject.SetActive(true);
            proj.Init(dir, weaponData, ownerStats.TotalStats.AttackPower, BattleManager.GetInstance.turretProjectilePool);
        }
    }
    private void Update()
    {
        RotateTowardNearestMonster();
    }
    private void RotateTowardNearestMonster()
    {
        Transform target = holder.FindNearestMonster(weaponData.Range);
        if (target == null) return;

        Vector2 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

