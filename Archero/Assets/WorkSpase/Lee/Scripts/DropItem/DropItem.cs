using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    Rigidbody2D rb;
    Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        OverlapRay();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    //=======================[Interaction RayCast]=======================
    void OverlapRay()
    {
        if (target == null)
        {
            Collider2D hit = Physics2D.OverlapCircle(this.transform.position, 2f, LayerMask.GetMask("Player"));
            if (hit != null)
            {
                target = hit.transform;
            }
        }
    }

    void FollowPlayer()
    {
        if (target != null)
        {
            // 플레이어 방향 계산
            Vector2 dir = ((Vector2)target.position - rb.position).normalized;
            // 원하는 만큼 속도 세팅
            rb.velocity = dir * 5f;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }

}
