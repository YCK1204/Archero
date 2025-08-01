using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingWeapon : WeaponBase
{
    [SerializeField] private float orbitRadius = 1.5f;
    [SerializeField] private float orbitSpeed = 90f; // degrees per second

    private float currentAngle;
    private Transform pivot; // ȸ�� �߽� (�÷��̾�)
    private CircleCollider2D hitbox;

    private void Awake()
    {
        pivot = transform.parent; // �÷��̾ �θ�� ����
        hitbox = GetComponent<CircleCollider2D>();
        if (hitbox != null) hitbox.isTrigger = true;
    }

    public override void Activate()
    {
        if (pivot == null) return;

        // ȸ�� ���� ����
        currentAngle += orbitSpeed * Time.deltaTime;
        if (currentAngle > 360f) currentAngle -= 360f;

        // ��ġ ���
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;
        transform.position = (Vector2)pivot.position + offset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            var monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                float damage = ownerStats.TotalStats.AttackPower * weaponData.AttackPower;
        //        monster.TakeDamage((int)damage);
            }
        }
    }
}
