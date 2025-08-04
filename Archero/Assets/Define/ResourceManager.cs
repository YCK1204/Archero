using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Defines
{
    public class ResourceManager : SingleTon<ResourceManager>//ALL : (250707)싱글톤 기반의 비동기 로딩 스크립트 가능하면 제가 캐싱해둔걸 사용하겠지만 알아두면 작업에 수월 할 것 같아 추가합니다
    {
        private bool isLoadAble<T>(T instance) { return instance != null; }
        //메니저 인스턴스 생성시 실행되는 함수
        public override void Init()
        {
            base.Init();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">리턴타입</typeparam>
        /// <param name="key">타겟 키값</param>
        /// <param name="callback">(obj)=>{targetInstance = obj}</param>
        public void LoadAsync<T>(string key, Action<T> callback, bool isCaching = false)
        {
            if (key.Contains(".sprite"))
            {
                key = $"{key}[{key.Replace(".sprite", "")}]";
            }
            AsyncOperationHandle<T> infoAsyncOP = Addressables.LoadAssetAsync<T>(key);
            infoAsyncOP.Completed += (op) =>
            {

                callback?.Invoke(infoAsyncOP.Result);
                if (isCaching) Addressables.Release(infoAsyncOP);
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">리턴타입</typeparam>
        /// <param name="label">타겟 키값</param>
        /// <param name="callback">(obj)=>{targetInstance = obj}</param>
        public void LoadAsyncAll<T>(string label, Action<(string, T)[]> callback, bool isCaching = true)
        {
            var labelKeys = Addressables.LoadResourceLocationsAsync(label, typeof(T));
            //label의 T타입인 오브젝트들의 키값을 가져온다
            labelKeys.WaitForCompletion();
            //resource를 전부 load할때까지 대기

            Debug.Log(labelKeys.Result);
            if (labelKeys.Result.Count == 0) { Debug.LogError($"{label}라벨이 비어있습니다."); callback.Invoke(null); }//해당하는 키가 없을경우 null을 리턴

            int doneCount = 0;

            (string, T)[] tempT = new (string, T)[labelKeys.Result.Count];
            for (int i = 0; i < tempT.Length; i++)
            {
                int curIndex = i;
                string curKey = labelKeys.Result[i].PrimaryKey; //콜백을 동시에 실행시 클로저 이슈가 생길 수 있기에 분리(다른 루프의 스택메모리를 참조하는 현상)
                LoadAsync<T>(labelKeys.Result[i].PrimaryKey, (result) =>
                {
                    tempT[curIndex].Item1 = curKey;
                    tempT[curIndex].Item2 = result;
                    doneCount++;
                    if (doneCount == labelKeys.Result.Count)
                    {
                        callback?.Invoke(tempT);
                        if (isCaching) Addressables.Release(labelKeys);
                    }
                }, isCaching);
            }
        }
        #region Json
        private string GetSavePath() => Application.persistentDataPath;

        public bool SaveData<T>(T data, string fileName, bool isOverride)
        {
            string path = Path.Combine(GetSavePath(), fileName);
            Debug.Log(path);
            if (File.Exists(path))
            {
                if (isOverride == true)
                {
                    File.WriteAllText(path, JsonConvert.SerializeObject(data));
                    return true;
                }
            }
            else
            {
                if (File.Exists(GetSavePath()))
                {
                    StreamWriter file = File.CreateText(path);
                    file.WriteLine(JsonConvert.SerializeObject(data));
                }
                else
                {
                    Directory.CreateDirectory(GetSavePath());
                    StreamWriter file = File.CreateText(path);
                    file.WriteLine(JsonConvert.SerializeObject(data));

                }
                return true;
            }
            return false;
        }
        public T LoadData<T>(string fileName) where T : new()
        {
            string path = Path.Combine(GetSavePath(), fileName);

            if (File.Exists(path))
            {
                T result;
                try
                {
                    result = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                }
                catch
                {
                    //파싱 실패시
                    result = default(T);
                }
                return result;
            }
            else
            {
                Directory.CreateDirectory(GetSavePath());
                return default(T);
            }
        }
        #endregion
    }
}
