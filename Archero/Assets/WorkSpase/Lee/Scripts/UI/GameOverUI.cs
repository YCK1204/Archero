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
        }

        void RetryGame()
        {
            SceneManager.LoadScene("DungeonScene");
        }
    }
}
