using Assets.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private int damage;
    private float timer;
    private BulletGroup bulletGroup;


    [SerializeField] private float lifetime = 3f;  // 막 쌓일 수 있대서, 일단 증발 시간 3f로 했어용
    [SerializeField] private LayerMask targetLayer;

    public void Init(Vector2 dir, WeaponData weaponData, int attackPower)
    {
        direction = dir.normalized;
        speed = weaponData.ProjectileSpeed;
        damage = attackPower;
        bulletGroup = GetComponentInParent<BulletGroup>();
        targetLayer = 1 << 7;
        timer = 0f;


        // 방향 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0)
            return;

        if ( collision.gameObject.layer == 7)
        {
            BattleManager.GetInstance.Attack(collision, damage, transform.position);
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        BattleManager.GetInstance.playerProjectilePool.EnQueue(this);
    }
}
