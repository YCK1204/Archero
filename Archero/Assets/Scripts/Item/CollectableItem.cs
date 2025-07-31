using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : Item
{
    protected Rigidbody2D _rb2d;
    protected BoxCollider2D _boxCollider2d;
    protected CircleCollider2D _circleCollider2d;
    protected PlayerController _player;
    float Speed = 6f;
    CollectableItemData _data => _itemData as CollectableItemData;
    public override void Use()
    {
        base.Use();
        var data = _itemData as CollectableItemData;
        data.Collect();
    }
    public override void Init(BaseItemData data)
    {
        base.Init(data);
        _rb2d = GetComponent<Rigidbody2D>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
        _circleCollider2d = GetComponent<CircleCollider2D>();
        _circleCollider2d.radius *= 5f;
    }
    private void FixedUpdate()
    {
        FixedUpdateController();
    }
    protected virtual void FixedUpdateController()
    {
        MoveToPlayer();
    }
    void MoveToPlayer()
    {
        if (_player == null)
            return;

        Vector3 playerPos = _player.transform.position;
        Vector3 direction = (playerPos - transform.position).normalized;
        transform.position += direction * Time.deltaTime * Speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_boxCollider2d.bounds.Contains(collision.bounds.max) || _boxCollider2d.bounds.Contains(collision.bounds.min))
            {
                _data.Collect();
                Object.Destroy(gameObject);
            }
            else
            {
                _player = collision.GetComponent<PlayerController>();
            }
        }
    }
}
