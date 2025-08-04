using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class SceneManagerEx : MonoBehaviour
{
    static SceneManagerEx _instance;
    public static SceneManagerEx Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("SceneManagerEx");
                _instance = obj.AddComponent<SceneManagerEx>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    public void LoadDungeonScene()
    {
        ResourceManager.GetInstance.LoadAsync<GameObject>("Dungeon", (go) =>
        {
            LoadSceneAsync("DungeonScene", () =>
            {
                var player = Instantiate(go);
                player.transform.position = Vector3.zero;
            });
        }, true);
    }
    /// <summary>
    /// 동기형 씬 로드 함수
    /// </summary>
    /// <param name="sceneName">로드할 씬 이름</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    /// <summary>
    /// 비동기형 씬 로드 함수
    /// </summary>
    /// <param name="sceneName">로드할 씬 이름</param>
    /// <param name="callback">콜백 함수</param>
    /// <param name="loop">콜백 함수가 로드될 동안 실행될지 로드된 후 실행될지 여부</param>
    public void LoadSceneAsync(string sceneName, Action callback = null)
    {
        SceneManager.LoadSceneAsync(sceneName).completed += (op) =>
        {
            callback?.Invoke();
        };

    }
    /// <summary>
    /// 비동기형 씬 로드 함수
    /// </summary>
    /// <param name="sceneName">로드할 씬 이름</param>
    /// <param name="progressCallback">로드될 동안 실행될 콜백 함수</param>
    /// <param name="callback">로드된 후 실행될 콜백 함수</param>
    public void LoadSceneAsync(string sceneName, Action<float> progressCallback, Action callback = null)
    {
        var oper = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(LoadSceneAsyncCoroutine(oper, progressCallback));
    }
    IEnumerator LoadSceneAsyncCoroutine(AsyncOperation oper, Action<float> callback)
    {
        while (!oper.isDone)
        {
            yield return null;
            callback?.Invoke(oper.progress);
        }
    }
}
