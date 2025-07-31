using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected BaseItemData _itemData;
    public Animator Animator => _itemData.Animator;
    public Sprite Icon => _itemData.Icon;
    public string Name => _itemData.Name;
    ItemType Type => _itemData.Type;

    /// <summary>
    /// 아이템을 타입에 따라 사용합니다.
    /// </summary>
    /// <param name = "player">플레이어</param>
    public virtual void Use(/*Player player*/) { }
    public virtual void Init(BaseItemData data)
    {
        _itemData = data;
    }
}
