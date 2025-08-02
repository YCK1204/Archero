using Assets.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;

    [SerializeField] private float lifetime = 3f;  // �� ���� �� �ִ뼭, �ϴ� ���� �ð� 3f�� �߾��
    [SerializeField] private LayerMask targetLayer;

    public void Init(Vector2 dir, WeaponData weaponData, int attackPower)
    {
        direction = dir.normalized;
        speed = weaponData.ProjectileSpeed;
        damage = attackPower;

        lifetime = 3f;
        targetLayer = 1 << 7;

        // ���� ȸ��
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0)
            return;

        if ( collision.gameObject.layer == 7)
        {
            BattleManager.GetInstance.Attack(collision, damage, transform.position);
        }

        Destroy(gameObject); // �⺻�� 1ȸ �浹 �� �ı�
    }
}
