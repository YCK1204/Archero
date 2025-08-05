using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lee.Scripts
{

    public class GameOverUI : PopUpUI
    {
        protected override void Awake()
        {
            base.Awake();
            buttons["ReturnButton"].onClick.AddListener(() => { RetryGame(); });
        }

        void RetryGame()
        {
            GameManager.UI.ClosePopUpUI();
            SceneManager.LoadScene("StartScene");
        }
    }
}
