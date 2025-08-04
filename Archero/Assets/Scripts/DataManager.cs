using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class DataManager : MonoBehaviour
{
    //static DataManager _instance;
    //public DataManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {

    //            GameObject obj = new GameObject("DataManager");
    //            _instance = obj.AddComponent<DataManager>();
    //        }
    //        return _instance;
    //    }
    //}
    [SerializeField]
    Item prefab;
    List<BaseItemData> _itemDataDict = new List<BaseItemData>();

    public void Start ()
    {
        ResourceManager.GetInstance.LoadAsyncAll<BaseItemData>("Item", (obj) =>
        {
            int y = 4;
            int x = 4;
            foreach (var item in obj)
            {
                _itemDataDict.Add(item.Item2); // 나중에 Dictionary로 관리 될수도
                if (item.Item2 is CollectableItemData collectableItem)
                {
                    var collectable = Instantiate(prefab);
                    collectable.Init(item.Item2);
                    collectable.transform.position = new Vector3(x, y, 0);
                    y *= -1; // y축을 기준으로 아이템 배치
                    x *= -1;
                }
            }
        });
    }
}
