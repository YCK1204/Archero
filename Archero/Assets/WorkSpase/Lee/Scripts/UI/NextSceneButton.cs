using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lee.Scripts
{
    public class NextSceneButton : MonoBehaviour
    {
        private void OnEnable()
        {
            GameManager.UI.Recreated();
        }
        public void StartSceneButton() 
        {
            SceneManager.LoadScene("MainScene");
        }

        public void DugeonSceneButton()
        {
            GameManager.Instance._clearCount = 23;
            SceneManager.LoadScene("DungeonScene");
        }

        public void CountClearTest()
        {
            GameManager.Instance.CheckStageClear();
        }

    }
}
