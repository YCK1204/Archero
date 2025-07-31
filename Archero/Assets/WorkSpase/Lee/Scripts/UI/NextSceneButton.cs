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

    }
}
