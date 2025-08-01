using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private WeaponData data;

    [SerializeField] private float lifetime = 3f;  // 막 쌓일 수 있대서, 일단 증발 시간 3f로 했어용

    public void Init(Vector2 dir, WeaponData weaponData)
    {
        direction = dir.normalized;
        data = weaponData;
        speed = data.ProjectileSpeed;
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
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        //{
        //    var monster = collision.GetComponent<Monster>();
        //    if (monster != null)
        //    {
        //        monster.TakeDamage(data.AttackPower);
        //    }

        //    if (!data.IsPiercing)
        //        Destroy(gameObject);
        //}
    }
}
