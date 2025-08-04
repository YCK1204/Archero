using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Collider2D Collider2D;
    [SerializeField]
    Sprite VerticalCloseSprite;
    [SerializeField]
    Sprite VerticalOpenSprite;
    [SerializeField]
    Sprite HorizontalCloseSprite;
    [SerializeField]
    Sprite HorizontalOpenSprite;
    public void Init(GameObject parent, Vector2Int startPos, Vector2Int endPos, bool start)
    {
        var renderer = GetComponent<SpriteRenderer>();
        Vector3 offset = new Vector3(0.5f, 0.5f, 0);
        bool horizontal = (endPos - startPos).x != 0;

        Vector2Int basePos = start ? startPos : endPos;
        transform.position = (Vector3Int)basePos + offset;

        if (horizontal)
        {
            renderer.sprite = HorizontalCloseSprite;

            bool leftToRight = startPos.x < endPos.x;
            bool placeLeft = (start && leftToRight) || (!start && !leftToRight);

            Collider2D.transform.position = transform.position + (placeLeft ? Vector3.left : Vector3.right);
            renderer.flipX = !placeLeft;
        }
        else
        {
            renderer.sprite = VerticalCloseSprite;

            bool bottomToTop = startPos.y < endPos.y;
            bool placeBottom = (start && bottomToTop) || (!start && !bottomToTop);

            Collider2D.transform.position = transform.position + (placeBottom ? Vector3.down : Vector3.up);
        }
    }
}
