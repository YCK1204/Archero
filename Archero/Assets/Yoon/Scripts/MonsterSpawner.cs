using Assets.Define;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(()=> MapManager.Instance.GetMapData != null && MapManager.Instance.GetMapData.Count > 0);
        List<Vector3> vec = new List<Vector3>();
        int monsterNumb = 1;
        for (int i = 0; i < MapManager.Instance.GetMapData.Count-1; i++)
        {
            SpawnQueue queue = new SpawnQueue();
            Map map = MapManager.Instance.GetMapData[i];
            for (int j = 0; j < monsterNumb; j++)
            {
                int randomX = UnityEngine.Random.Range(-10, 10);
                int randomY = UnityEngine.Random.Range(-10, 10);
                ChessCharType type = (ChessCharType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ChessCharType)).Length-1);
                Vector3 pos = new Vector3(map.CenterPosition.x+ randomX, map.CenterPosition.y+randomY, 0);
                List<Vector3> patrols = new List<Vector3>();
                if(!map.Positions.Contains(new Vector2Int(map.CenterPosition.x + randomX, map.CenterPosition.y + randomY)))
                {
                    j--;
                    continue;
                }
                for (int k = 0; k < 3; k++)
                {
                    int patrolX = UnityEngine.Random.Range(-10, 10);
                    int patrolY = UnityEngine.Random.Range(-10, 10);
                    Vector3 patrolPos = new Vector3(map.CenterPosition.x + patrolX, map.CenterPosition.y + patrolY, 0);
                    if (!map.Positions.Contains(new Vector2Int(map.CenterPosition.x + randomX, map.CenterPosition.y + randomY)) || patrols.Contains(patrolPos))
                    {
                        k--;
                        continue;
                    }
                    patrols.Add(patrolPos);
                }
                queue.types.Enqueue((ChessCharType.King, new Vector3(map.CenterPosition.x, map.CenterPosition.y, 0), null));
                queue.types.Enqueue((type, pos,patrols.ToArray()));
            }
            monsterNumb += 2;

            BattleManager.GetInstance.spawnQueue.Enqueue(queue);
        }
        Vector2Int centerPos = MapManager.Instance.GetMapData[MapManager.Instance.GetMapData.Count - 1].CenterPosition;
        BattleManager.GetInstance.spawnQueue.Enqueue(new SpawnQueue(ChessCharType.King, new Vector3(centerPos.x,centerPos.y,0),null));

        BattleManager.GetInstance.SpawnMonster();

    }
}
