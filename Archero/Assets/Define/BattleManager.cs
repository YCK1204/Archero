using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Define
{
    class BattleManager :SingleTon<BattleManager>
    {
        Dictionary<Collider2D, Action<int, Vector3>> unitDict = new Dictionary<Collider2D, Action<int, Vector3>>();
        public Pool<MobProjectile> normalMobProjectile;
        public override void Init()
        {
            base.Init();
            unitDict = new Dictionary<Collider2D, Action<int, Vector3>>();
            normalMobProjectile.Init();
        }
        public void Attack(Collider2D target,int damage,Vector3 attackerPos)
        {
            if (unitDict.ContainsKey(target)) unitDict[target].Invoke(damage, attackerPos);
        }
        public void RegistHitInfo(Collider2D target, Action<int, Vector3> action)
        {
            unitDict.Add(target, action);
        }
        
    }
    public class Pool<T> where T : MonoBehaviour
    {
        private Queue<T> queue;
        private T prefab;
        public void Init()
        {
            queue = new Queue<T>();
        }
        public Pool(string str)
        {
            ResourceManager.GetInstance.LoadAsync<GameObject>(str, (result) =>
            {
                prefab = result.GetComponent<T>();
            });
        }
        public void EnQueue(T obj)
        {
            obj.gameObject.SetActive(false);
            queue.Enqueue(obj);
        }
        public T DeQueue()
        {
            if(queue.Count <= 0)//큐언더플로우 방지
            {
                GameObject gameObj = GameObject.Instantiate(prefab.gameObject);
                if (!gameObj.TryGetComponent(typeof(T), out Component result))
                    result = gameObj.AddComponent<T>();
                return (T)result;
            }
            T obj = queue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
    }
}
