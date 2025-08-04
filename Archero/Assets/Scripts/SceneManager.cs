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
    /// ������ �� �ε� �Լ�
    /// </summary>
    /// <param name="sceneName">�ε��� �� �̸�</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    /// <summary>
    /// �񵿱��� �� �ε� �Լ�
    /// </summary>
    /// <param name="sceneName">�ε��� �� �̸�</param>
    /// <param name="callback">�ݹ� �Լ�</param>
    /// <param name="loop">�ݹ� �Լ��� �ε�� ���� ������� �ε�� �� ������� ����</param>
    public void LoadSceneAsync(string sceneName, Action callback = null)
    {
        SceneManager.LoadSceneAsync(sceneName).completed += (op) =>
        {
            callback?.Invoke();
        };

    }
    /// <summary>
    /// �񵿱��� �� �ε� �Լ�
    /// </summary>
    /// <param name="sceneName">�ε��� �� �̸�</param>
    /// <param name="progressCallback">�ε�� ���� ����� �ݹ� �Լ�</param>
    /// <param name="callback">�ε�� �� ����� �ݹ� �Լ�</param>
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
