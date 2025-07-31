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
        public Pool<Monster> monsterPool;
        public override void Init()
        {
            base.Init();
            unitDict = new Dictionary<Collider2D, Action<int, Vector3>>();

            normalMobProjectile = new Pool<MobProjectile>("MonsterArrow");
            normalMobProjectile.Init();
            monsterPool = new Pool<Monster>("MonsterBase");
            monsterPool.Init();
        }
        public void Attack(Collider2D target,int damage,Vector3 attackerPos)
        {
            if (unitDict.ContainsKey(target)) unitDict[target].Invoke(damage, attackerPos);
            else Debug.LogError("등록되지 않은 키 값입니다. BattleManager의 RegistHitInfo 함수를 통해 Collider2D값을 등록해주세요" +
                "모르겠으면 허윤<< 찌르세요");
        }
        public void RegistHitInfo(Collider2D target, Action<int, Vector3> action)
        {
            unitDict.Add(target, action);
        }
        
    }
    
}
public class Pool<T> where T : MonoBehaviour
{
    private Stack<T> stack;
    private T prefab;
    public void Init()
    {
        stack = new Stack<T>();
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
        stack.Push(obj);
    }
    public T DeQueue()
    {
        if (stack.Count <= 0)//큐언더플로우 방지
        {
            GameObject gameObj = GameObject.Instantiate(prefab.gameObject);
            if (!gameObj.TryGetComponent(typeof(T), out Component result))
                result = gameObj.AddComponent<T>();
            return (T)result;
        }
        T obj = stack.Pop();
        obj.gameObject.SetActive(true);
        return obj;
    }
}