using Assets.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeapon : WeaponBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float orbitDistance = 1.5f;

    private float lastAttackTime = -Mathf.Infinity;


    public override void Activate()
    {
        if (Time.time - lastAttackTime < weaponData.Cooldown / ownerStats.TotalStats.AttackSpeed)
        {
            return;
        }

        Transform target = holder.FindNearestMonster(weaponData.Range);
        if (target == null)
        {
            return;
        }

        FireAt(target.position);
        lastAttackTime = Time.time;
    }

    private void FireAt(Vector2 targetPos)
    {
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
        OrbitBehindPlayer();
    }
    private void RotateTowardNearestMonster()
    {
        if (holder == null || weaponData == null) return;

        Transform target = holder.FindNearestMonster(weaponData.Range);
        if (target == null) return;

        Vector2 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 좌우 판별
        bool flip = dir.x < 0;

        // Flip 처리 (Sprite 좌우 반전)
        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
        transform.localScale = scale;

        // 회전 방향 보정 (flip되었으면 z축도 반대로)
        if (flip)
            angle += 180f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OrbitBehindPlayer()
    {
        Transform target = holder.FindNearestMonster(weaponData.Range);
        if (target == null) return;

        // 플레이어 → 몬스터 방향
        Vector2 dirToTarget = (target.position - holder.transform.position).normalized;

        // 반대 방향으로 이동한 위치
        Vector3 orbitOffset = -dirToTarget * orbitDistance;

        orbitOffset += Vector3.up * 0.6f;

        // 터렛의 위치를 플레이어 기준으로 이동
        transform.position = holder.transform.position + orbitOffset;
    }


}

