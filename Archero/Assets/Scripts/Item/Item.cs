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
    /// �������� Ÿ�Կ� ���� ����մϴ�.
    /// </summary>
    /// <param name = "player">�÷��̾�</param>
    public virtual void Use(/*Player player*/) { }
    public virtual void Init(BaseItemData data)
    {
        _itemData = data;
    }
}
