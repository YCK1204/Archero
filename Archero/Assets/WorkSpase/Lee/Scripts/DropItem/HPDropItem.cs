using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDropItem : DropItem
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<CharacterStats>().Heal(1);
        }
    }
}
