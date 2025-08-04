using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lee.Scripts
{
    public class NextSceneButton : MonoBehaviour
    {
        public void StartSceneButton()
        {
            SceneManager.LoadScene("MainScene");
        }
        public void LeaveGameButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        public void DugeonSceneButton()
        {
            GameManager.Instance._clearCount = 23; // 23개 방을 클리어해야 함
            SceneManagerEx.Instance.LoadSceneAsync("DungeonScene", () =>
            {
                Defines.ResourceManager.GetInstance.LoadAsync<GameObject>("Player", (go) =>
                {
                    MapManager.Instance.GenerateMap();
                    var player = Instantiate(go);
                    player.transform.position = Vector3.zero;
                }, true);
            });
        }
    }
}
