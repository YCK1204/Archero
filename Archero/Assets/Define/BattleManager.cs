using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Defines;
using Lee.Scripts;

namespace Assets.Define
{
    class BattleManager :SingleTon<BattleManager>
    {
        Dictionary<Collider2D, Action<int, Vector3>> unitDict = new Dictionary<Collider2D, Action<int, Vector3>>();
        public Dictionary<Collider2D, Action<int, Vector3>> GetUnitDIct { get { return unitDict; } }
        public Pool<MobProjectile> normalMobProjectile;
        public Dictionary<ChessCharType,Pool<Monster>> monsterPool;
        public Pool<Projectile> playerProjectilePool;                      // 0804 추가 by 김정민
        public Pool<Projectile> turretProjectilePool;                      // 0804 추가 by 김정민
        public Pool<DropItem>[] Items;
        public Queue<SpawnQueue> spawnQueue = new Queue<SpawnQueue>();


        public int stageNum { get { return 23-GameManager.Instance._clearCount; } }

        public override void Init()
        {
            base.Init();
            unitDict = new Dictionary<Collider2D, Action<int, Vector3>>();

            normalMobProjectile = new Pool<MobProjectile>("MonsterArrow");
            monsterPool = new Dictionary<ChessCharType, Pool<Monster>>();

            monsterPool.Add(ChessCharType.pawn, new Pool<Monster>("Pawn"));
            monsterPool.Add(ChessCharType.bishop, new Pool<Monster>("Bishop"));
            monsterPool.Add(ChessCharType.knight, new Pool<Monster>("Knight"));
            monsterPool.Add(ChessCharType.rock, new Pool<Monster>("Rock"));
            monsterPool.Add(ChessCharType.King, new Pool<Monster>("King"));
            playerProjectilePool = new Pool<Projectile>("Bullet");         // 0804 추가 by 김정민
            turretProjectilePool = new Pool<Projectile>("Bullet_Turret");  // 0804 추가 by 김정민
            Items = new Pool<DropItem>[2] { new Pool<DropItem>("ExpItem"), new Pool<DropItem>("HPItem") };
        }
        public void Attack(Collider2D target,int damage,Vector3 attackerPos)
        {
            //if (unitDict.ContainsKey(target) == false)
            //{
            //    unitDict.Add(target, );
            //}

            if (unitDict.ContainsKey(target)) unitDict[target].Invoke(damage, attackerPos);
            else Debug.LogError("등록되지 않은 키 값입니다. BattleManager의 RegistHitInfo 함수를 통해 Collider2D값을 등록해주세요" +
                "모르겠으면 허윤<< 찌르세요");
        }
        public void RegistHitInfo(Collider2D target, Action<int, Vector3> action)
        {
            unitDict.Add(target, action);
        }
        public void RemoveHitInfo(Collider2D target)
        {
            unitDict.Remove(target);
        }
        public void SpawnMonster()
        {
            if (!spawnQueue.TryDequeue(out SpawnQueue result)) return;

            while (result.types.Count>0)
            {
                (ChessCharType, Vector3,Vector3[]) spawnData = result.types.Dequeue();
                monsterPool[spawnData.Item1].DeQueue().Spawn(spawnData.Item3,spawnData.Item1);
                
            }
        }
    }
}
public class Pool<T> where T : MonoBehaviour
{
    public bool IsPrefabReady => prefab != null;
    private Stack<T> stack;
    private T prefab;
    public void Init()
    {
        stack = new Stack<T>();
    }
    public Pool(string str)
    {
        Init();
        if (str == string.Empty) return;
        Defines.ResourceManager.GetInstance.LoadAsync<GameObject>(str, (result) =>
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
        if (prefab == null)
        {
            Debug.LogError($"[Pool<{typeof(T).Name}>] prefab이 아직 준비되지 않았습니다.");
            return null;
        }
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
public class SpawnQueue
{
    public Queue<(ChessCharType, Vector3, Vector3[])> types;
    public SpawnQueue()
    {
        types = new Queue<(ChessCharType,Vector3,Vector3[])>();
    }
    public SpawnQueue(ChessCharType type ,Vector3 vec, Vector3[] patrolPath)
    {
        types = new Queue<(ChessCharType, Vector3, Vector3[])>();
        types.Enqueue((type, vec,patrolPath));
    }
}