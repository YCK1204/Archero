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
    private Pool<Projectile> originPool;

    [SerializeField] private float lifetime = 3f;  // �� ���� �� �ִ뼭, �ϴ� ���� �ð� 3f�� �߾��
    [SerializeField] private LayerMask targetLayer;

    public virtual void Init(Vector2 dir, WeaponData weaponData, int attackPower, Pool<Projectile> returnPool)
    {
        direction = dir.normalized;
        speed = weaponData.ProjectileSpeed;
        damage = attackPower;
        bulletGroup = GetComponentInParent<BulletGroup>();
        targetLayer = 1 << 7;
        timer = 0f;
        originPool = returnPool;

        // ���� ȸ��
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
        if (originPool != null)
        {
            originPool.EnQueue(this);
        }
        else
        {
            Debug.LogWarning("Projectile: originPool�� null�Դϴ�. Ǯ�� �ǵ��� �� �����ϴ�.");
            Destroy(gameObject); // ���� ��ġ
        }
    }
    private void OnDisable()
    {
        transform.position = new Vector3(10000f, 10000f, 10000f);
    }

    
}
