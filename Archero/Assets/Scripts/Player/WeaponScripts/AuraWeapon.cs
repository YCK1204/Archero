using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraWeapon : WeaponBase
{
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask monsterLayer;

    private float lastTickTime = -Mathf.Infinity;
    private Transform center;

    private void Awake()
    {
        center = transform.parent; // 플레이어 기준
    }

    public override void Activate()
    {
        if (Time.time < lastTickTime + tickInterval) return;

        DealTickDamage();
        lastTickTime = Time.time;
    }

    private void DealTickDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center.position, radius, monsterLayer);
        foreach (var hit in hits)
        {
            Monster monster = hit.GetComponent<Monster>();
            if (monster != null)
            {
                float damage = ownerStats.TotalStats.AttackPower * weaponData.AttackPower;
        //        monster.TakeDamage((int)damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (center == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(center.position, radius);
    }
}

