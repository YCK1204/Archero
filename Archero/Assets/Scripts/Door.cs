using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Collider2D Collider2D;
    public void SetTriggerPosition(Vector2Int direction)
    {
        Collider2D.transform.position = transform.position + (Vector3Int)direction;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Event
    }
}
