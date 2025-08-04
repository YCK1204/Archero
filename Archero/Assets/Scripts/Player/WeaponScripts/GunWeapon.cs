using Assets.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    [SerializeField] private Transform firePoint;

    private float lastAttackTime = -Mathf.Infinity;

    private void Update()
    {
        RotateTowardNearestMonster();
    }

    public override void Activate()
    {
        //if (weaponData == null) Debug.LogWarning("weaponData is null");
        //if (ownerStats == null) Debug.LogWarning("ownerStats is null");
        //if (holder == null) Debug.LogWarning("holder is null");

        if (weaponData == null || ownerStats == null || holder == null)
            return;
        if (Time.time - lastAttackTime < weaponData.Cooldown / ownerStats.TotalStats.AttackSpeed)
            return;

        Transform target = holder.FindNearestMonster(weaponData.Range);
        if (target == null) return;

        FireAt(target.position);
        lastAttackTime = Time.time;
    }

    private void FireAt(Vector2 targetPos)
    {
        // 1. ���� Ȯ�� (�÷��̾� Ǯ�� ���� �غ� �ȵ� ��� ���)
        var pool = BattleManager.GetInstance?.playerProjectilePool;
        if (pool == null)
        {
            Debug.LogWarning("GunWeapon: Projectile Pool�� ���� �غ���� �ʾҽ��ϴ�.");
            return;
        }

        // 2. Ǯ�� �غ�ƴ��� (prefab�� �Ҵ�ƴ���) ���� üũ
        bool testProj = pool.IsPrefabReady;
        if (!testProj)
        {
            Debug.LogWarning("GunWeapon: Ǯ���� ����ü�� �������� ���߽��ϴ�.");
            return;
        }
        // 3. ��ġ ���� �� ���
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
            Projectile proj = pool.DeQueue();
            if (proj == null)
            {
                Debug.LogWarning("GunWeapon: Ǯ���� ����ü�� �������� ���߽��ϴ�. (�ݺ� ��)");
                continue;
            }
            proj.transform.position = firePoint.position;
            proj.transform.rotation = Quaternion.Euler(0, 0, angle);
            proj.gameObject.SetActive(true);
            proj.Init(dir, weaponData, ownerStats.TotalStats.AttackPower, BattleManager.GetInstance.playerProjectilePool);

        }
    }

    private void RotateTowardNearestMonster()
    {
        if (holder == null || weaponData == null) return;

        Transform target = holder.FindNearestMonster(weaponData.Range);
        if (target == null) return;

        Vector2 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // �¿� �Ǻ�
        bool flip = dir.x < 0;

        // Flip ó�� (Sprite �¿� ����)
        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
        transform.localScale = scale;

        // ȸ�� ���� ���� (flip�Ǿ����� z�൵ �ݴ��)
        if (flip)
            angle += 180f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}