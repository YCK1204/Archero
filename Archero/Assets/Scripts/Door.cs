using Assets.Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Collider2D Collider2D;
    [SerializeField]
    Sprite OpenSprite;
    bool isActivate = false;
    Sprite CloseSprite;
    public void Init(GameObject parent, Vector2Int startPos, Vector2Int endPos, bool start)
    {
        var renderer = GetComponent<SpriteRenderer>();
        Vector3 offset = new Vector3(0.5f, 0.5f, 0);
        bool horizontal = (endPos - startPos).x != 0;

        Vector2Int basePos = start ? startPos : endPos;
        transform.position = (Vector3Int)basePos + offset;

        renderer.sprite = CloseSprite;
        if (horizontal)
        {
            bool leftToRight = startPos.x < endPos.x;
            bool placeLeft = (start && leftToRight) || (!start && !leftToRight);

            Collider2D.transform.position = transform.position + (placeLeft ? Vector3.left : Vector3.right);
        }
        else
        {
            bool bottomToTop = startPos.y < endPos.y;
            bool placeBottom = (start && bottomToTop) || (!start && !bottomToTop);

            Collider2D.transform.position = transform.position + (placeBottom ? Vector3.down : Vector3.up);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isActivate && collision.gameObject.layer == 3)
        {
            BattleManager.GetInstance.SpawnMonster();
            
            isActivate = true;
            Collider2D.isTrigger = false;
            //TODO : 문 수집해둔게 있으면 전체적으로 잠그는것도 괜찮아보입니다!
        }
    }
}
