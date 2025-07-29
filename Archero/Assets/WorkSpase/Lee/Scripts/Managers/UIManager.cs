using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;


namespace Lee.Scripts
{
    public class UIManager : MonoBehaviour
    {
        private Canvas windowCanvas;

        private void Awake()
        {
            windowCanvas = GameManager.Resource.Instantiate<Canvas>("Prefabs/UI/Canvas");
            windowCanvas.gameObject.name = "WindowCanvas";
            windowCanvas.sortingOrder = 10;
        }

        public void Recreated()
        {
            windowCanvas = GameManager.Resource.Instantiate<Canvas>("Prefabs/UI/Canvas");
            windowCanvas.gameObject.name = "WindowCanvas";
            windowCanvas.sortingOrder = 10;
        }

        public void Clear()
        {
            GameManager.Resource.Destroy(windowCanvas);
        }

        public T ShowWindowUI<T>(T windowUI) where T : WindowUI
        {
            T ui = GameManager.Pool.GetUI(windowUI);
            ui.transform.SetParent(windowCanvas.transform, false);
            return ui;
        }

        public T ShowWindowUI<T>(string path) where T : WindowUI
        {
            T ui = GameManager.Resource.Load<T>(path);
            return ShowWindowUI(ui);
        }

        public void CloseWindowUI(WindowUI windowUI)
        {
            GameManager.Pool.ReleaseUI(windowUI.gameObject);
        }

        public void SelectWindowUI<T>(T windowUI) where T : WindowUI
        {
            windowUI.transform.SetAsLastSibling();
        }
    }
}
