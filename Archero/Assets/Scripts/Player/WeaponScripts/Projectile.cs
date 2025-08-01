using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float damage;

    [SerializeField] private float lifetime = 3f;  // �� ���� �� �ִ뼭, �ϴ� ���� �ð� 3f�� �߾��
    [SerializeField] private LayerMask targetLayer;

    public void Init(Vector2 dir, WeaponData weaponData, float attackPower)
    {
        direction = dir.normalized;
        speed = weaponData.ProjectileSpeed;
        damage = attackPower;

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

        //Monster monster = collision.GetComponent<Monster>();
        //if (monster != null)
        //{
        //    monster.TakeDamage((int)damage);
        //}

        Destroy(gameObject); // �⺻�� 1ȸ �浹 �� �ı�
    }
}
