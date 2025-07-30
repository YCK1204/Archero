using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingleTon<DataManager>
{
    List<BaseItemData> _itemDataDict = new List<BaseItemData>();

    public override void Init()
    {
        ResourceManager.GetInstance.LoadAsyncAll<BaseItemData>("Item", (obj) =>
        {
            foreach (var item in obj)
            {
                _itemDataDict.Add(item.Item2); // 나중에 Dictionary로 관리 될수도
            }
        });
    }
}
